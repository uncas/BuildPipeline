namespace Uncas.BuildPipeline.ApplicationServices
{
    using System.Collections.Generic;
    using Uncas.BuildPipeline.ApplicationServices.Results;
    using Uncas.BuildPipeline.Models;
    using Uncas.Core.Data;

    public interface IDeploymentService
    {
        void Deploy(int pipelineId, int environmentId);

        void DeployDueDeployments();

        IEnumerable<Deployment> GetDeployments(int pipelineId);

        IEnumerable<Deployment> GetDeploymentsByEnvironment(int environmentId);

        IEnumerable<Deployment> GetDueDeployments(PagingInfo pagingInfo);

        ScheduleDeploymentResult ScheduleDeployment(
            int pipelineId,
            int environmentId);
    }
}