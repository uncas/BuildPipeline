using System.Diagnostics.CodeAnalysis;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Uncas.BuildPipeline.Utilities
{
    public class ZipUtility : IZipUtility
    {
        #region IZipUtility Members

        [SuppressMessage(
            "Microsoft.Usage",
            "CA2202:Do not dispose objects multiple times",
            Justification = "Unknown in zip implementation.")]
        public void ExtractZipFile(
            string sourcePackagePath,
            string destinationRootFolderPath)
        {
            if (!File.Exists(sourcePackagePath))
                return;

            if (Directory.Exists(destinationRootFolderPath))
                Directory.Delete(destinationRootFolderPath, true);

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

        #endregion

        private static void ExtractZipEntry(
            string destinationRootFolderPath,
            ZipInputStream zipStream,
            ZipEntry theEntry)
        {
            string fileName = Path.GetFileName(theEntry.Name);
            if (string.IsNullOrEmpty(fileName))
                return;

            string directoryName = Path.GetDirectoryName(theEntry.Name);
            if (string.IsNullOrEmpty(directoryName))
                return;

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
                        streamWriter.Write(data, 0, size);
                    else
                        break;
                }
            }
        }
    }
}