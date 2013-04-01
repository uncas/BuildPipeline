using System.Collections.Generic;
using Uncas.BuildPipeline.Models;
using Uncas.Core.Data;

namespace Uncas.BuildPipeline.Repositories
{
    public interface IDeploymentRepository
    {
        void AddDeployment(Deployment deployment);
        IEnumerable<Deployment> GetByEnvironment(int environmentId);
        Deployment GetDeployment(int deploymentId);
        IEnumerable<Deployment> GetDeployments(int pipelineId);
        IEnumerable<Deployment> GetDueDeployments(PagingInfo pagingInfo);
        void UpdateDeployment(Deployment deployment);
    }
}