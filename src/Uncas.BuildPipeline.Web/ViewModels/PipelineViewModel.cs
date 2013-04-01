﻿using System.Collections.Generic;

namespace Uncas.BuildPipeline.Web.ViewModels
{
    public class PipelineViewModel
    {
        public string CreatedDisplay { get; set; }
        public string CssClass { get; set; }
        public int PipelineId { get; set; }
        public string ProjectName { get; set; }
        public string SourceAuthor { get; set; }
        public string Revision { get; set; }
        public string BranchName { get; set; }
        public string StatusText { get; set; }
        public IEnumerable<BuildStepViewModel> Steps { get; set; }
        public string PackagePath { get; set; }
        public string CommitLink { get; set; }
    }
}