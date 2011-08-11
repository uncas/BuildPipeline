namespace Uncas.BuildPipeline.Utilities
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
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
            var startInfo =
                new ProcessStartInfo(
                    "Deploy.cmd") { WorkingDirectory = workingDirectory };
            Process.Start(startInfo);
        }

        private static void ExtractZipEntry(
            string destinationRootFolderPath,
            ZipInputStream zipStream,
            ZipEntry theEntry)
        {
            string fileName = Path.GetFileName(theEntry.Name);
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            string directoryName = Path.GetDirectoryName(theEntry.Name);
            if (string.IsNullOrEmpty(directoryName))
            {
                return;
            }

            // create directory
            string destinationFolderPath =
                Path.Combine(
                    destinationRootFolderPath,
                    directoryName);
            Directory.CreateDirectory(destinationFolderPath);

            string destinationFilePath = Path.Combine(destinationFolderPath, fileName);
            using (FileStream streamWriter = File.Create(destinationFilePath))
            {
                var data = new byte[2048];
                while (true)
                {
                    int size = zipStream.Read(data, 0, data.Length);
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

        [SuppressMessage(
            "Microsoft.Usage", 
            "CA2202:Do not dispose objects multiple times",
            Justification = "Unknown in zip implementation.")]
        private static void ExtractZipFile(
            string sourcePackagePath,
            string destinationRootFolderPath)
        {
            if (!File.Exists(sourcePackagePath))
            {
                return;
            }

            if (Directory.Exists(destinationRootFolderPath))
            {
                Directory.Delete(destinationRootFolderPath, true);
            }

            using (FileStream baseInputStream = File.OpenRead(sourcePackagePath))
            {
                using (var zipStream = new ZipInputStream(baseInputStream))
                {
                    ZipEntry theEntry;
                    while ((theEntry = zipStream.GetNextEntry()) != null)
                    {
                        ExtractZipEntry(
                            destinationRootFolderPath,
                            zipStream,
                            theEntry);
                    }
                }
            }
        }

        private static void WriteEnvironmentProperties(
            Environment environment,
            string workingDirectory)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<properties>");
            foreach (EnvironmentProperty property in environment.Properties)
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
    }
}