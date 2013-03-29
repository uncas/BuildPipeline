function LogPackage (
        $baseUrl = "http://localhost:51743",
        $projectName = "BuildPipeline",
        $branchName = "master",
        $revision = (git rev-parse HEAD),
        $stepName = "Package",
        $packagePath) {
    if (!$branchName -or ($branchName -eq "HEAD")) {
        Write-Host "Logging skipped, since no specific branch was given."
        return
    }
    if (!(Test-Path $packagePath)) {
        Write-Host "Logging skipped, since no package was found at '$packagePath'."
        return
    }

    $packageFile = gci $packagePath
    $packageFileName = $packageFile.Name

    try {
        $filePostUrl = "$baseUrl/customapi/packages/$packageFileName"
        UploadFile $packagePath $filePostUrl
        
        $logBaseUrl = "$baseUrl/api/pipelines?format=json"
        $logParameters = "projectname=$projectName&branchName=$branchName&revision=$revision&stepname=$stepName&packagepath=$packageFileName"
        $logUrl = "$logBaseUrl&$logParameters"
        $pipelineId = (Invoke-RestMethod -Uri $logUrl -Method POST)
    }
    catch {
        # Logging shouldn't break the build.
    }
}