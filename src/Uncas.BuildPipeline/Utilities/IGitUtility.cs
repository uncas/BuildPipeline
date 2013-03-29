using System.Collections.Generic;

namespace Uncas.BuildPipeline.Utilities
{
    public interface IGitUtility
    {
        IEnumerable<string> GetBranchesMerged(
            string repository, string from, string to, params string[] excludeBranches);

        IEnumerable<GitLog> GetLogs(
            string localMirror,
            string from,
            string to,
            string gitHubUrl,
            int limit = 1000,
            bool includeMerges = false);

        string GetShortLog(string repository, string from, string to);
        IEnumerable<string> GetChangedFiles(string repository, string @from, string to);

        /// <summary>
        /// Checks whether the first commit contains the second commit.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="first">The first commit.</param>
        /// <param name="second">The second commit.</param>
        /// <returns>True if the first commit contains the second commit.</returns>
        bool ContainsCommit(string repository, string first, string second);

        void Mirror(string sourceUrl, string mirrorsFolder, string mirrorName);
    }
}