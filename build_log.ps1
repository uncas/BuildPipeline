$url = "http://localhost:51743/api/pipelines?projectname=test&branchname=master2&revision=2&stepname=package&packagepath=bla.zip&format=json"
Invoke-RestMethod -Uri $url -Method POST

# function LogPackage (
        # $projectName = "BuildPipeline",
        # $branchName = "master",
        # $revision = (git rev-parse HEAD),
        # $stepName = "Package",
        # $packagePath) {
    # Write-Host "Logging start..."
    # $baseApiUrl = "http:blabla"
    # CallEndPoint $projectName $branchName $revision $stepName $packagePath
# }
    