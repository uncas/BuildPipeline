namespace Uncas.BuildPipeline.Web.ViewModels
{
    using System.Collections.Generic;

    public class PipelineViewModel
    {
        public string CreatedDisplay { get; set; }
        public string CssClass { get; set; }
        public string ProjectName { get; set; }
        public int SourceRevision { get; set; }
        public string SourceUrlRelative { get; set; }
        public string StatusText { get; set; }
        public IEnumerable<BuildStepViewModel> Steps { get; set; }
    }
}