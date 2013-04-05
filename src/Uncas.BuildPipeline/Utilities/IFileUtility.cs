namespace Uncas.BuildPipeline.Utilities
{
    public interface IFileUtility
    {
        bool FileExists(string filePath);
        void WriteAllText(string filePath, string contents);
    }
}