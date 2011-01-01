namespace Uncas.BuildPipeline.Repositories
{
    using System.Collections.Generic;
    using Uncas.BuildPipeline.Models;

    public class DeploymentRepository : IDeploymentRepository
    {
        public void AddDeployment(Deployment deployment)
        {
        }

        public IEnumerable<Deployment> GetScheduledDeployments()
        {
            return null;
        }

        public IEnumerable<Deployment> GetDeployments(int pipelineId)
        {
            return null;
        }
    }
}