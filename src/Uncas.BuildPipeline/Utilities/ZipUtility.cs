using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
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
                throw new ArgumentException("Package path does not exist.",
                                            "sourcePackagePath");

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

        /// <remarks>
        /// From https://github.com/icsharpcode/SharpZipLib/wiki/Zip-Samples
        /// </remarks>
        public void CreateZipFile(string sourceFolderPath, string zipFilePath)
        {
            using (FileStream fsOut = File.Create(zipFilePath))
            {
                using (var zipStream = new ZipOutputStream(fsOut))
                {
                    // 0-9, 9 being the highest level of compression
                    zipStream.SetLevel(3);

                    // This setting will strip the leading part of the folder path in the entries, to
                    // make the entries relative to the starting folder.
                    // To include the full path for each entry up to the drive root, assign folderOffset = 0.
                    int folderOffset = sourceFolderPath.Length +
                                       (sourceFolderPath.EndsWith("\\") ? 0 : 1);

                    CompressFolder(sourceFolderPath, zipStream, folderOffset);

                    zipStream.IsStreamOwner = true;
                    zipStream.Close();
                }
            }
        }

        #endregion

        private void CompressFolder(
            string path,
            ZipOutputStream zipStream,
            int folderOffset)
        {
            string[] files = Directory.GetFiles(path);

            foreach (string filename in files)
            {
                var fi = new FileInfo(filename);

                string entryName = filename.Substring(folderOffset);

                // Makes the name in zip based on the folder
                entryName = ZipEntry.CleanName(entryName);

                // Removes drive from name and fixes slash direction
                var newEntry = new ZipEntry(entryName)
                    {DateTime = fi.LastWriteTime, Size = fi.Length};

                zipStream.PutNextEntry(newEntry);

                // Zip the file in buffered chunks
                var buffer = new byte[4096];
                using (FileStream streamReader = File.OpenRead(filename))
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                zipStream.CloseEntry();
            }

            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders)
                CompressFolder(folder, zipStream, folderOffset);
        }

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