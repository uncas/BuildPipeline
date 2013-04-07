using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using NUnit.Framework;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Tests.Integration.Utilities
{
    public class ZipUtilityTests : WithBootstrapping<IZipUtility>
    {
        private const string DestinationRootFolderPath = @"C:\Temp\testzipfolder";

        [Test]
        public void ExtractZipFile_FileNotFound_ArgumentException()
        {
            const string zipFilePath = @"C:\Temp\tesxxxxxxxxtzip.txt";

            Assert.Throws<ArgumentException>(
                () => Sut.ExtractZipFile(zipFilePath, DestinationRootFolderPath));
        }

        [Test]
        public void ExtractZipFile_TextFile_ZipException()
        {
            const string zipFilePath = @"C:\Temp\testzip.txt";
            File.WriteAllText(zipFilePath, "bla bla");

            Assert.Throws<ZipException>(
                () => Sut.ExtractZipFile(zipFilePath, DestinationRootFolderPath));
        }

        [Test]
        public void ExtractZipFile_ZipFile_RunsWithoutErrors()
        {
            const string sourceFilesPath = @"C:\Temp\testzipsource";
            const string zipFilePath = @"C:\Temp\testzip.zip";
            if (Directory.Exists(sourceFilesPath))
                Directory.Delete(sourceFilesPath, true);
            Directory.CreateDirectory(sourceFilesPath);
            File.WriteAllText(Path.Combine(sourceFilesPath, "test.txt"), "bla bla");
            string subFolderPath = Path.Combine(sourceFilesPath, "SubItems");
            if (!Directory.Exists(subFolderPath))
                Directory.CreateDirectory(subFolderPath);
            File.WriteAllText(Path.Combine(subFolderPath, "subtest.txt"), "bla bla");
            Sut.CreateZipFile(sourceFilesPath, zipFilePath);

            Sut.ExtractZipFile(zipFilePath, DestinationRootFolderPath);
        }
    }
}