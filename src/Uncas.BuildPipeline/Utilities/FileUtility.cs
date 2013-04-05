using System.IO;

namespace Uncas.BuildPipeline.Utilities
{
    public class FileUtility : IFileUtility
    {
        #region IFileUtility Members

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public void WriteAllText(string filePath, string contents)
        {
            File.WriteAllText(filePath, contents);
        }

        #endregion
    }
}