Build Pipeline
==============

Build Pipeline is a tool that manages and visualizes builds in a pipeline.

Current features:
- Website that lists builds and deployment packages.
- API endpoint and powershell script to log builds and packages
  from a build server.

Planned features:
- Trigger deployment to a specific environment.
- Display commit info from git.


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


Development
-----------
Development requirements:
- 'Installation requirements' (see above)
- Visual Studio 2012
