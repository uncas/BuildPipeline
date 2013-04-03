Build Pipeline
==============

Build Pipeline is a tool that manages and visualizes builds in a pipeline.

Current features:
- Website that lists builds and deployment packages.
- API endpoint and powershell script to log builds and packages
  from a build server.
- Trigger deployment to a specific environment.
- Deployment method: use a powershell script to perform the deployment,
  where the powershell script can be defined by the user in the website.
- Quick-link to Github.
- Display commit info for projects that depend on a git repository.
- Configuration of github URL and remote Git URL for projects.

Planned features:
- Deployment method: FTP
- See further feature ideas in doc/todo.txt


Installation
------------
Installation requirements:
- Internet access (downloads nuget packages from nuget.org)
- MS SQL Server (Express)
- IIS 7
- IIS WebAdministration powershell snapin
  (can be installed with Web Platform Installer)
- .NET 4

Installation steps:
- Run build.ps1 to build and deploy the website to the local machine.
- It will be setup in IIS locally at http://BuildPipeline at port 80.
- Add '127.0.0.1    buildpipeline' to the hosts file
  (C:\Windows\System32\Drivers\etc\hosts).
- As default the script creates a database named 'BuildPipeline'
  in the local 'SqlExpress' instance.
- The setup script can be modified to setup differently
  (to be described better how to do that, ask me if required).


Usage
-----
Configuring an existing project for Build Pipeline:
- Create a build script that compiles and packages your project,
  see for example build.ps1 in this project.
- Call the API endpoint to tell Build Pipeline that a build has completed,
  and that a deployment package is available,
  see for example build_log.ps1 in this project.
- Set up a build in a build server,
  for example CruiseControl.NET or TeamCity,
  that calls the build script.
- After the first build of the project,
  it should be possible to go to 'Projects' in Build Pipeline,
  and define 'Deployment script' (powershell),
  Github URL (e.g. 'https://github.com/uncas/BuildPipeline'),
  and Git remote URL (e.g. 'git://github.com/uncas/BuildPipeline.git').

Daily usage:
- When you commit changes in your project,
  and your build server has built from the commit,
  then a pipeline should appear in Build Pipeline.
- If you would like to deploy from a given commit,
  then choose 'Deploy' and select target environment.


Development
-----------
Development requirements:
- 'Installation requirements' (see above)
- Visual Studio 2012
