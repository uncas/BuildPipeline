namespace Uncas.BuildPipeline.Repositories
{
    using System.Collections.Generic;
    using Uncas.BuildPipeline.Models;
    using Uncas.Core.Data;

    public interface IDeploymentRepository
    {
        void AddDeployment(Deployment deployment);
        IEnumerable<Deployment> GetByEnvironment(int environmentId);
        Deployment GetDeployment(int id);
        IEnumerable<Deployment> GetDeployments(int pipelineId);
        IEnumerable<Deployment> GetDueDeployments(PagingInfo pagingInfo);
        void UpdateDeployment(Deployment deployment);
    }
}