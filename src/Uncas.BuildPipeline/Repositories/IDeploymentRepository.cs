namespace Uncas.BuildPipeline.Repositories
{
    using System.Collections.Generic;
    using Uncas.BuildPipeline.Models;

    public interface IDeploymentRepository
    {
        void AddDeployment(Deployment deployment);
        IEnumerable<Deployment> GetDueDeployments(PagingInfo pagingInfo);
        IEnumerable<Deployment> GetDeployments(int pipelineId);
        IEnumerable<Deployment> GetByEnvironment(int environmentId);
        void UpdateDeployment(Deployment deployment);
        Deployment GetDeployment(int id);
    }

    public class PagingInfo
    {
        public PagingInfo(int pageSize)
        {
            PageSize = pageSize;
        }

        public int PageSize { get; private set; }
    }
}
