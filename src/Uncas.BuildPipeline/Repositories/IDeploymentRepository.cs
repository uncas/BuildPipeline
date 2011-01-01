namespace Uncas.BuildPipeline.Repositories
{
    using System.Collections.Generic;
    using Uncas.BuildPipeline.Models;

    public interface IDeploymentRepository
    {
        void AddDeployment(Deployment deployment);
        IEnumerable<Deployment> GetDueDeployments();
        IEnumerable<Deployment> GetDeployments(int pipelineId);
    }
}
