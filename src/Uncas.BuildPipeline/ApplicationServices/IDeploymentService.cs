namespace Uncas.BuildPipeline.ApplicationServices
{
    using System.Collections.Generic;
    using Uncas.BuildPipeline.ApplicationServices.Results;
    using Uncas.BuildPipeline.Models;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.Core.Data;

    public interface IDeploymentService
    {
        void Deploy(int pipelineId, int environmentId);
        
        void DeployDueDeployments();
        
        IEnumerable<Deployment> GetDueDeployments(PagingInfo pagingInfo);
        
        IEnumerable<Deployment> GetDeployments(int pipelineId);

        ScheduleDeploymentResult ScheduleDeployment(
            int pipelineId, 
            int environmentId);

        IEnumerable<Deployment> GetDeploymentsByEnvironment(int environmentId);
    }
}