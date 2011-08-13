namespace Uncas.BuildPipeline.Web.ViewModels
{
    using System;

    public class DeploymentViewModel
    {
        public DateTime Created { get; set; }
        
        public DateTime? Completed { get; set; }
        
        public DateTime? Started { get; set; }
        
        public string EnvironmentName { get; set; }
    }
}