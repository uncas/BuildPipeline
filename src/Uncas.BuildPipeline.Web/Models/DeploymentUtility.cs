namespace Uncas.BuildPipeline.Web.Models
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using ICSharpCode.SharpZipLib.Zip;

    public class DeploymentUtility
    {
        public void Deploy(
            string packagePath,
            string workingDirectory,
            int environmentId)
        {
            ExtractZipFile(packagePath, workingDirectory);
            WriteEnvironmentProperties(environmentId, workingDirectory);
            DeployPackage(workingDirectory);
        }

        private static void DeployPackage(string workingDirectory)
        {
            // TODO: Low Priority: Consider fallback to use the following command if Deploy.cmd is not found:
            // "C:\Program Files (x86)\NAnt\nant.exe" -buildfile:BuildActions.build deploy

            // Runs command on package:
            ProcessStartInfo startInfo =
                new ProcessStartInfo(
                "Deploy.cmd");
            startInfo.WorkingDirectory = workingDirectory;
            Process.Start(startInfo);
        }

        private void WriteEnvironmentProperties(
            int environmentId,
            string workingDirectory)
        {
            // TODO: Store the names of the properties in a list for each project.
            string websiteDestinationPath =
                @"c:\inetpub\wwwroot\Uncas.BuildPipeline.Web";
            string websiteName = "BuildPipelineWeb";
            int websitePort = 876;
            if (environmentId == 2)
            {
                websiteDestinationPath =
                    @"c:\inetpub\wwwroot\Uncas.BuildPipeline.Web.QA";
                websiteName = "BuildPipelineWeb-QA";
                websitePort = 872;
            }

            // TODO: Allow editing values of properties for each separate environment.
            string format = @"<?xml version=""1.0""?>
<properties>

  <property name=""website.destination.path"" value=""{0}"" />
  <property name=""website.name"" value=""{1}"" />
  <property name=""website.port"" value=""{2}"" />

</properties>";
            string properties = string.Format(
                format,
                websiteDestinationPath,
                websiteName,
                websitePort);
            string filePath =
                Path.Combine(workingDirectory, "local.properties.xml");
            File.WriteAllText(filePath, properties);
        }

        private void ExtractZipFile(
            string sourcePackagePath,
            string destinationRootFolderPath)
        {
            if (!File.Exists(sourcePackagePath))
            {
                Console.WriteLine("Cannot find file '{0}'", sourcePackagePath);
                return;
            }

            if (Directory.Exists(destinationRootFolderPath))
                Directory.Delete(destinationRootFolderPath, true);

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(sourcePackagePath)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    Console.WriteLine(theEntry.Name);

                    // create directory
                    string destinationFolderPath = Path.Combine(destinationRootFolderPath, directoryName);
                    Directory.CreateDirectory(destinationFolderPath);

                    if (fileName != String.Empty)
                    {
                        string destinationFilePath = Path.Combine(destinationFolderPath, fileName);
                        using (FileStream streamWriter = File.Create(destinationFilePath))
                        {

                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}