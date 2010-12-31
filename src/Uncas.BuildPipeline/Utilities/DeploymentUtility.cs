namespace Uncas.BuildPipeline.Utilities
{
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using ICSharpCode.SharpZipLib.Zip;
    using Uncas.BuildPipeline.Models;

    public class DeploymentUtility : IDeploymentUtility
    {
        public void Deploy(
            string packagePath,
            string workingDirectory,
            Environment environment)
        {
            ExtractZipFile(packagePath, workingDirectory);
            WriteEnvironmentProperties(environment, workingDirectory);
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
            Environment environment,
            string workingDirectory)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<properties>");
            foreach (var property in environment.Properties)
            {
                sb.AppendFormat(
                    "  <property name=\"{0}\" value=\"{1}\" />",
                    property.Key,
                    property.Value);
                sb.AppendLine();
            }

            sb.AppendLine("</properties>");
            string properties = sb.ToString();
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

                    // create directory
                    string destinationFolderPath = Path.Combine(destinationRootFolderPath, directoryName);
                    Directory.CreateDirectory(destinationFolderPath);

                    if (fileName != string.Empty)
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