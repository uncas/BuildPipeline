# Original from https://github.com/ayende/rhino-mocks/blob/master/psake_ext.ps1

function Clean-Folder {
    param(
        [string]$folder = $(throw "folder is required")
    )
    if (Test-Path $folder)
    {
        rmdir -force -recurse $folder
    }
}

function Remove-Folder-Contents {
    param (
        [string]$folder = $(throw "folder is required")
    )

    if (Test-Path $folder)
    {
        Remove-Item -Path $folder\* -recurse -force
    }
    else
    {
        mkdir $folder
    }
}

function Sync-Folders
{
    param (
        [string]$source = $(throw "source is required"),
        [string]$destination = $(throw "destination is required")
    )

    Remove-Folder-Contents $destination
    copy $source\* $destination -recurse
}

function Copy-WebApplication {
    param (
        [string]$sourceParentFolder = $(throw "source parent folder is required"),
        [string]$webApplicationName = $(throw "web application name is required"),
        [string]$destinationParentFolder = $(throw "destination parent folder is required")
     )

     mkdir "$collectDir\$webApplicationName"

    $webProjectFile = "$sourceParentFolder\$webApplicationName\$webApplicationName.csproj"
    msbuild $webProjectFile /p:"Configuration=$configuration;PipelineDependsOnBuild=True;RunCodeAnalysis=false" /p:WebProjectOutputDir=$collectDir\$webApplicationName\ /p:OutDir=$destinationParentFolder\$webApplicationName\bin\ /t:Rebuild /t:ResolveReferences /t:_WPPCopyWebApplication
}

function Get-Git-Commit
{
    $git = "C:\Program Files (x86)\git\cmd\git.cmd"
    $gitLog = & "$git" log --oneline -1
    return $gitLog.Split(' ')[0]
}

function Get-Git-CommitCount
{
    $git = "C:\Program Files (x86)\git\cmd\git.cmd"
    $gitLog = & "$git" log --pretty=oneline
    return $gitLog.length
}

function Generate-Assembly-Info
{
param(
	[string]$company, 
	[string]$product, 
	[string]$copyright, 
	[string]$version,
	[string]$file = $(throw "file is a required parameter.")
)
  $commit = Get-Git-Commit
  $commitCount = Get-Git-CommitCount
  $fullVersion = "$version.$commitCount"
  $script:fullVersion = $fullVersion
  "Version $fullVersion (commit hash: $commit, commit log count: $commitCount)"
  $asmInfo = "using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyCompanyAttribute(""$company"")]
[assembly: AssemblyProductAttribute(""$product"")]
[assembly: AssemblyCopyrightAttribute(""$copyright"")]
[assembly: AssemblyVersionAttribute(""$fullVersion"")]
[assembly: AssemblyInformationalVersionAttribute(""$fullVersion-$commit"")]
[assembly: AssemblyFileVersionAttribute(""$fullVersion"")]
[assembly: AssemblyDelaySignAttribute(false)]
"

	$dir = [System.IO.Path]::GetDirectoryName($file)
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}
	Write-Host "Generating assembly info file: $file"
	out-file -filePath $file -encoding UTF8 -inputObject $asmInfo
}

function Run-Test
{
    param(
        [string]$testProjectName = $(throw "file is a required parameter."),
        [string]$outDir = $(throw "out dir is a required parameter.")
    )

    $testResultFile = "$outDir\$testProjectName.TestResult.xml"
    & $nunitExe "$testDir\$testProjectName\bin\$configuration\$testProjectName.dll" /xml=$testResultFile

    if ($lastExitCode -ne 0) {
        throw "One or more failures in tests - see details above."
    }
}

function GetAndStartStopwatch {
    return [System.Diagnostics.Stopwatch]::StartNew()
}

function StopAndWriteStopwatch ($stopwatch, $description) {
    $stopwatch.Stop()
    $totalSeconds = $stopwatch.Elapsed.TotalSeconds
    Write-Host "$totalSeconds seconds: $description"
}

function ReplaceConnectionString ($configFilePath, $connectionString) {
    $xml = [xml](get-content $configFilePath)
    $root = $xml.get_DocumentElement();
    $root.connectionStrings.add.connectionString = $connectionString
    $xml.Save($configFilePath)
}

function FlipRemoteWebSite($siteName, $physicalPath, $webserver) {
    $session = New-PSSession -ComputerName $webserver
    if (!$session) { return }
    $block = { 
        param($siteName, $physicalPath)
        if (!$physicalPath -or !$siteName) { return }
        Add-PSSnapin WebAdministration
        Set-ItemProperty "IIS:\Sites\$siteName" -name physicalPath -value $physicalPath
    }
    Invoke-Command -Session $session -ScriptBlock $block -argumentlist $siteName, $physicalPath
}

function FlipWebSite($siteName, $physicalPath, $webserver = $webserver) {
    if ($webserver -ne "localhost") {
        FlipRemoteWebSite $siteName $physicalPath $webServer
        return
    }

    Set-ItemProperty "IIS:\Sites\$siteName" -name physicalPath -value $physicalPath
}

function DeployWeb (
        $sourceWebFolder = $sourceWebFolder,
        $webRootName = $webRootName,
        $webServer = $webServer,
        $connectionString = $connectionString) {
    ReplaceConnectionString "$sourceWebFolder\web.config" $connectionString

    $siteName = $webRootName
    $relativeWebRoot = "inetpub\wwwroot\$webRootName"
    $remoteWebRoot = "\\$webserver\c$\$relativeWebRoot"

    $sha = (git rev-parse HEAD)
    $webFolder = "$sha"
    $localWebFolder = "C:\$relativeWebRoot\$webFolder"
    $remoteWebFolder = "$remoteWebRoot\$webFolder"

    # Deploying files to website:
    $sw = GetAndStartStopwatch
    if (!(Test-Path $remoteWebRoot)) { mkdir $remoteWebRoot }
    $previousWebsiteVersions = (gci $remoteWebRoot)
    Sync-Folders $sourceWebFolder $remoteWebFolder
    StopAndWriteStopwatch $sw "Deploy files to website"

    Import-Module WebAdministration

    # Creating site if it does not already exist:
    $iisPath = "IIS:\Sites\$siteName"
    if (!(Test-Path $iisPath)) {
        $sw = GetAndStartStopwatch
        New-Item $iisPath -bindings @{protocol="http";bindingInformation=":80:$siteName"} -physicalPath $localWebFolder
        StopAndWriteStopwatch $sw "Create website"
        return
    }
    
    # TODO: Consider warming the new location up?

    # Flipping website:
    $sw = GetAndStartStopwatch
    FlipWebSite $siteName $localWebFolder
    StopAndWriteStopwatch $sw "Flip website"

    # Removing old versions of website:
    $sw = GetAndStartStopwatch
    foreach ($item in $previousWebsiteVersions) {
        if ($item.Mode -eq "d----") {
            $fullName = $item.FullName
            # Do not remove current version, relevant if we're re-deploying same version as last time:
            if ($fullName.Contains($webFolder)) { continue }
            Write-Host "Removing $fullName"
            rmdir $fullName -force -recurse
        }
    }
    StopAndWriteStopwatch $sw "Remove old versions of website"
}

function DownloadFile ($from, $to) {
    Write-Host "Downloading file from '$from' to '$to'."
    $webClient = New-Object System.Net.WebClient
    $script = $webClient.DownloadString($from)
    Set-Content $to $script
}

function ThrowIfErrorOnLastExecution {
    if ($LASTEXITCODE -ne 0) {
        throw "Error - last exit code was $LASTEXITCODE!"
    }
}

#function Replace-File
#{
#    param {
#        [string]$sourceFile = $(throw "source file is required"),
#        [string]$targetFile = $(throw "target file is required"),
#        [string]$originalValue = $(throw "original value is required"),
#        [string]$finalValue = $(throw "final value is required")
#    }
#
#    (cat $sourceFile) -replace '$originalValue', "$finalValue" > $targetFile
#}


# Notes on getting powershell IIS remoting to work:

# On the client (build server):
# - List trusted hosts: Get-Item WSMan:\localhost\Client\TrustedHosts
# - Use server name, not IP.
# - Configure trusted hosts: winrm set winrm/config/client '@{TrustedHosts="my-server"}'

# On the host (web server):
# - Install IIS WebAdministration powershell snapin, can be done with Web Platform Installer
# - Check if there is a listener: Get-ChildItem WSMan:\localhost\Listener
# - Add listener: Enable-PSRemoting –force
