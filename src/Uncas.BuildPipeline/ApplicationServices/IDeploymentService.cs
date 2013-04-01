using System.Collections.Generic;
using Uncas.BuildPipeline.Models;
using Uncas.Core.Data;

namespace Uncas.BuildPipeline.ApplicationServices
{
    public interface IDeploymentService
    {
        void Deploy(int pipelineId, int environmentId);
        void DeployDueDeployments();
        IEnumerable<Deployment> GetDeployments(int pipelineId);
        IEnumerable<Deployment> GetDeploymentsByEnvironment(int environmentId);
        IEnumerable<Deployment> GetDueDeployments(PagingInfo pagingInfo);
    }
}