namespace Uncas.BuildPipeline.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Uncas.BuildPipeline.Models;

    public class FakeDeploymentRepository : IDeploymentRepository
    {
        private static IList<Deployment> deployments =
            new List<Deployment>();

        public void AddDeployment(Deployment deployment)
        {
            deployments.Add(deployment);
        }

        public IEnumerable<Deployment> GetDueDeployments()
        {
            return deployments.Where(d => !d.HasRun);
        }

        public IEnumerable<Deployment> GetDeployments(int pipelineId)
        {
            return deployments.Where(d => d.PipelineId == pipelineId);
        }
    }
}