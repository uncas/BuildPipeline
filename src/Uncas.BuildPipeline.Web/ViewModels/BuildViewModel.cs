namespace Uncas.BuildPipeline.Web.ViewModels
{
    public class BuildViewModel
    {
        public string ProjectName { get; set; }
        public int SourceRevision { get; set; }
        public string SourceUrlRelative { get; set; }
        public BuildStepViewModel StepCommit { get; set; }
        public BuildStepViewModel StepAcceptance { get; set; }
    }
}