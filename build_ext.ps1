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