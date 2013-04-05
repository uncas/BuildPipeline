namespace Uncas.BuildPipeline.Utilities
{
    public interface IZipUtility
    {
        void ExtractZipFile(
            string sourcePackagePath,
            string destinationRootFolderPath);

        void CreateZipFile(
            string sourceFolderPath,
            string zipFilePath);
    }
}