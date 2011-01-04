namespace Uncas.BuildPipeline.Web.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class DeployViewModel
    {
        public IEnumerable<EnvironmentViewModel> Environments { get; set; }
        public IEnumerable<DeploymentViewModel> Deployments { get; set; }
        public PipelineViewModel Pipeline { get; set; }
    }

    public class DeploymentViewModel
    {
        public DateTime Created { get; set; }
        public DateTime? Completed { get; set; }
        public DateTime? Started { get; set; }
        public string EnvironmentName { get; set; }
    }
}