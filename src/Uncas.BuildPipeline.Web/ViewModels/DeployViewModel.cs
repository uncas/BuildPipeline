namespace Uncas.BuildPipeline.Web.ViewModels
{
    using System.Collections.Generic;

    public class DeployViewModel
    {
        public IEnumerable<EnvironmentViewModel> Environments { get; set; }
        
        public IEnumerable<DeploymentViewModel> Deployments { get; set; }
        
        public PipelineViewModel Pipeline { get; set; }
    }
}