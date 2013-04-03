using System;

namespace Uncas.BuildPipeline.Models
{
    public class CommitReadModel
    {
        public int ProjectId { get; set; }
        public string Revision { get; set; }

        public string Subject { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public DateTime AuthorDate { get; set; }
    }
}