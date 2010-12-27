namespace Uncas.BuildPipeline.Web.ViewModels
{
    public class BuildViewModel
    {
        public string ProjectName { get; set; }
        public int SourceRevision { get; set; }
        public BuildStepViewModel StepUnit { get; set; }
        public BuildStepViewModel StepIntegration { get; set; }
    }
}