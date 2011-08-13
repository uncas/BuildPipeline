namespace Uncas.BuildPipeline.ApplicationServices.Results
{
    using Uncas.BuildPipeline.Models;

    public class ScheduleDeploymentResult : BaseResult
    {
        public ScheduleDeploymentResult(Deployment deployment)
        {
            Success = true;
            Deployment = deployment;
        }

        public ScheduleDeploymentResult()
        {
        }

        public Deployment Deployment { get; set; }
    }
}