param (
    [string]$task = "Deploy",
    [string]$branch = (git rev-parse --abbrev-ref HEAD),
    [string]$dbScriptVersion = "1db33b6ea826630b496e6b9a0d347eca425cf00b",
    [string]$environment = "develop",
    [string]$baseUrl = "http://localhost:51743"
)

# Constants for this solution:
$solutionName = "Uncas.BuildPipeline"
$serviceName = "Uncas.BuildPipeline.WindowsService"
$nunitVersion = "2.6.0.12051"
$versionMajor = 1
$versionMinor = 0
$versionBuild = 0
$configuration = "Release"
$webserver = "localhost"

# Environment settings:
$databaseName = "BuildPipeline"
$webRootName = "BuildPipeline"
$serviceRootName = $serviceName
if ($environment -ne "prod") {
    $databaseName = "develop_$databaseName"
    $webRootName = "develop.$webRootName"
    $serviceRootName = "develop-$serviceName"
}

# Working variables:
$baseDir  = Resolve-Path .
$srcDir = "$baseDir\src"
$testDir = "$baseDir\test"
$outputDir = "$baseDir\output"
$collectDir = "$outputDir\collect"
$solutionFile = "$baseDir\$solutionName.sln"
$nunitFolder = "$baseDir\packages\NUnit.Runners.$nunitVersion\tools"
$nunitExe = "$nunitFolder\nunit-console.exe"
$year = (Get-Date).year
$connectionString = "Server=.\SqlExpress;Database=$databaseName;Integrated Security=true;"
$testDatabaseName = $databaseName + "_test"
$testConnectionString = "Server=.\SqlExpress;Database=$testDatabaseName;Integrated Security=true;"
$sourceWebFolder = "$collectDir\Uncas.BuildPipeline.Web"

. "$baseDir\build_ext.ps1"
. "$baseDir\build_log.ps1"

function Clean {
    Clean-Folder $collectDir
    Clean-Folder $outputDir
}

function Init {
    Clean
    
    if (!(Test-Path $outputDir))
    {
        mkdir $outputDir
    }
    if (!(Test-Path $collectDir))
    {
        mkdir $collectDir
    }

    Generate-Assembly-Info `
        -file "$baseDir\src\VersionInfo.cs" `
        -company "Uncas" `
        -product "$solutionName" `
        -version "$versionMajor.$versionMinor.$versionBuild" `
        -copyright "Copyright (c) $year, Uncas"
}

function ThrowIfErrorOnLastExecution {
    if ($LASTEXITCODE -ne 0) {
        throw "Error - last exit code was $LASTEXITCODE!"
    }
}

function Compile {
    Init
    msbuild $solutionFile /p:Configuration=$configuration /p:RunCodeAnalysis=false /t:Rebuild
    ThrowIfErrorOnLastExecution
}

function UnitTest {
    Compile
    Run-Test "Uncas.BuildPipeline.Tests.Unit" $outputDir
}

function UpdateDb ($connectionString) {
    $scriptSource = "https://raw.github.com/uncas/db-deployment/$dbScriptVersion/dbDeployment.ps1"
    $script = "packages\dbDeployment-$dbScriptVersion.ps1"
    #$script = "C:\Projects\OpenSource\db-deployment\dbDeployment.ps1"
    if (!(Test-Path $script)) {
        DownloadFile $scriptSource $script
    }
    . $script -connectionString $connectionString -scriptpath sql -clearScreenOnStart $false
}

function UpdateDbs {
    UpdateDb $connectionString
    UpdateDb $testConnectionString
}

function IntegrationTest {
    UnitTest
    UpdateDbs
    $testProjectName = "Uncas.BuildPipeline.Tests.Integration"
    ReplaceConnectionString "$testDir\$testProjectName\bin\$configuration\$testProjectName.dll.config" $testConnectionString
    Run-Test $testProjectName $outputDir
}

function Collect {
    IntegrationTest
    
    Copy-WebApplication $srcDir "Uncas.BuildPipeline.Web" $collectDir
    copy $srcDir\Uncas.BuildPipeline.WindowsService\bin\$configuration $collectDir\Uncas.BuildPipeline.WindowsService -recurse

    $params = @{branchName=$branch; packagePath="none.zip"; baseUrl=$baseUrl}
    LogPackage @params
}

function DeployService (
        $destinationMachine = ".", 
        $destinationRootFolder = "C:\Services",
        $serviceRootName) {
    if ($environment -ne "prod") { return }
    $service = Get-Service -ComputerName $destinationMachine -Name "$serviceRootName*"
    if ($service -and ($service.Status -ne "Stopped")) {
        # Stopping service:
        $sw = GetAndStartStopwatch
        $service.Stop()
        StopAndWriteStopwatch $sw "Stop service"

        # Waiting for the service to be completely shut down:
        $sw = GetAndStartStopwatch
        $waited = 0
        $waitInterval = 1
        $waitLimit = 120
        while ($service -and ($service.Status -ne "Stopped")) {
            Start-Sleep -s $waitInterval
            $waited += $waitInterval
            if ($waited -gt $waitLimit) { break }
            $service = Get-Service -ComputerName $destinationMachine -Name $serviceRootName
        }
        # Extra time to wait, such that files are not in use anymore:
        Start-Sleep -s 5
        StopAndWriteStopwatch $sw "Wait for service shut-down"
    }

    # Deploying files to service:
    $destinationFolder = "$destinationRootFolder\$serviceRootName"
    ReplaceConnectionString "$collectDir\$serviceName\$serviceName.exe.config" $connectionString
    $sw = GetAndStartStopwatch
    Sync-Folders "$collectDir\$serviceName" $destinationFolder
    StopAndWriteStopwatch $sw "Deploy files to service"

    # Installing service:
    if (!$service) {
        New-Service -name $serviceRootName -binaryPathName "$destinationFolder\$serviceName.exe"
        $service = Get-Service -ComputerName $destinationMachine -Name $serviceRootName
    }
    
    # Starting service:
    $sw = GetAndStartStopwatch
    $service.Start()
    StopAndWriteStopwatch $sw "Start service"
}

function Deploy {
    Collect
    DeployService
    DeployWeb
}

if ($task -and (Test-Path "Function:\$task")) {
    Write-Host "Invoking the task '$task' for branch '$branch'."
    Invoke-Expression "$task"
}
else {
    Write-Host "The task '$task' was not found."
}

if ($LASTEXITCODE -ne 0) {
    throw "Build error - last exit code was $LASTEXITCODE!"
}

# Notes on getting powershell IIS remoting to work:

# On the client (build server):
# - List trusted hosts: Get-Item WSMan:\localhost\Client\TrustedHosts
# - Use server name, not IP.
# - Configure trusted hosts: winrm set winrm/config/client '@{TrustedHosts="my-server"}'

# On the host (web server):
# - Install IIS WebAdministration powershell snapin, can be done with Web Platform Installer
# - Check if there is a listener: Get-ChildItem WSMan:\localhost\Listener
# - Add listener: Enable-PSRemoting –force
