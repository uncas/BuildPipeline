namespace Uncas.BuildPipeline.Utilities
{
    public static class GitHubLink
    {
        public static string Branch(string repo, string branch)
        {
            if (string.IsNullOrWhiteSpace(repo))
                return string.Empty;
            return string.Format("{0}/tree/{1}", repo, branch);
        }

        public static string Compare(string repo, string branch)
        {
            if (string.IsNullOrWhiteSpace(repo))
                return string.Empty;
            return string.Format("{0}/compare/{1}?w=1", repo, branch);
        }

        public static string Compare(string repo, string fromRevision, string toRevision)
        {
            if (string.IsNullOrWhiteSpace(repo))
                return string.Empty;
            return string.Format("{0}/compare/{1}...{2}?w=1",
                                 repo,
                                 fromRevision,
                                 toRevision);
        }

        public static string Revision(string repo, string revision)
        {
            if (string.IsNullOrWhiteSpace(repo))
                return string.Empty;
            return string.Format("{0}/commit/{1}?w=1", repo, revision);
        }
    }
}