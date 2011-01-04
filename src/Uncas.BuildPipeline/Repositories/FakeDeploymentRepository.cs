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
            int newId = 1;
            if (deployments.Count > 0)
            {
                newId = deployments.Max(d => d.Id.Value) + 1;
            }

            deployment.ChangeId(newId);
            deployments.Add(deployment);
        }

        public Deployment GetDeployment(int id)
        {
            return deployments.SingleOrDefault(d => d.Id == id);
        }

        public IEnumerable<Deployment> GetDeployments(int pipelineId)
        {
            return deployments.Where(d => d.PipelineId == pipelineId);
        }

        public IEnumerable<Deployment> GetDueDeployments()
        {
            return deployments.Where(d => !d.HasRun);
        }

        public void UpdateDeployment(Deployment deployment)
        {
        }


        public IEnumerable<Deployment> GetByEnvironment(int environmentId)
        {
            throw new System.NotImplementedException();
        }
    }
}