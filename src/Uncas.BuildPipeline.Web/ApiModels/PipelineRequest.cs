using ServiceStack.ServiceHost;

namespace Uncas.BuildPipeline.Web.ApiModels
{
    [Route("/pipelines")]
    [Route("/pipelines", "POST")]
    [Route("/pipelines/{Id}", "PUT")]
    public class PipelineRequest : IReturn<PipelineRequest>
    {
        public int PipelineId { get; set; }
        public string ProjectName { get; set; }
        public string BranchName { get; set; }
        public string Revision { get; set; }
        public string StepName { get; set; }
        public string PackagePath { get; set; }
    }
}