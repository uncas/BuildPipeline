namespace Uncas.BuildPipeline.ApplicationServices.Results
{
    using Uncas.BuildPipeline.Models;

    public class ScheduleDeploymentResult : BaseResult
    {
        public ScheduleDeploymentResult(Deployment deployment)
        {
            this.Success = true;
            this.Deployment = deployment;
        }

        public ScheduleDeploymentResult()
        {
        }

        public Deployment Deployment { get; set; }
    }
}