namespace Uncas.BuildPipeline.Web.ViewModels
{
    public class EnvironmentIndexViewModel
    {
        public int EnvironmentId { get; set; }
        
        public string EnvironmentName { get; set; }
        
        public int CurrentSourceRevision { get; set; }
    }
}