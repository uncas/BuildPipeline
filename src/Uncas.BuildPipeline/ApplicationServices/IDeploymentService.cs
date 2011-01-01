namespace Uncas.BuildPipeline.ApplicationServices
{
    using System.Collections.Generic;
    using Uncas.BuildPipeline.ApplicationServices.Results;
    using Uncas.BuildPipeline.Models;

    public interface IDeploymentService
    {
        void Deploy(int pipelineId, int environmentId);
        void DeployDueDeployments();
        IEnumerable<Deployment> GetDueDeployments();
        IEnumerable<Deployment> GetDeployments(int pipelineId);

        ScheduleDeploymentResult ScheduleDeployment(
            int pipelineId, 
            int environmentId);
    }
}