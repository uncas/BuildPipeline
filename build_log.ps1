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
    # TODO: Post/Put file to web service
    $endpointUrl = "$baseUrl/api/pipelines?format=json"
    $url = "$endpointUrl&projectname=$projectName&branchName=$branchName&revision=$revision&stepname=$stepName&packagepath=$packagePath"
    try {
        $pipelineId = (Invoke-RestMethod -Uri $url -Method POST)
    }
    catch {
        # Logging shouldn't break the build.
    }
}