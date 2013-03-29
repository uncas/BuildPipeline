using System;

namespace Uncas.BuildPipeline.Utilities
{
    public class GitLog
    {
        public string Revision { get; set; }
        public string Subject { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public DateTime AuthorDate { get; set; }
        public string RevisionLink { get; set; }

        public override string ToString()
        {
            return string.Format("Revision {0}. Author: {1} ({2}). Subject: {3}",
                                 Revision,
                                 AuthorName,
                                 AuthorEmail,
                                 Subject);
        }
    }
}