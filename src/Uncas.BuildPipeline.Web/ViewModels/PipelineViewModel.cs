namespace Uncas.BuildPipeline.Web.ViewModels
{
    using System.Collections.Generic;

    public class PipelineViewModel
    {
        public string ProjectName { get; set; }
        public int SourceRevision { get; set; }
        public string SourceUrlRelative { get; set; }
        public string CreatedDisplay { get; set; }
        public IEnumerable<BuildStepViewModel> Steps { get; set; }
    }
}