function LogPackage (
        $projectName = "BuildPipeline",
        $branchName = "master",
        $revision = (git rev-parse HEAD),
        $stepName = "Package",
        $packagePath) {
    Write-Host "Logging start..."
    $baseApiUrl = "http:blabla"
    CallEndPoint $projectName $branchName $revision $stepName $packagePath
}
    