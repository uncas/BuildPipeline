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
            string workingDirectory)
        {
            // 1) Extract package to some working directory
            ExtractZipFile(packagePath, workingDirectory);

            // 2) Run command on package:
            ProcessStartInfo startInfo =
                new ProcessStartInfo(
                "Deploy.cmd");
            startInfo.WorkingDirectory = workingDirectory;
            Process.Start(startInfo);
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