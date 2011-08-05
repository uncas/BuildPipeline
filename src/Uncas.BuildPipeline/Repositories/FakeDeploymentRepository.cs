namespace Uncas.BuildPipeline.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Uncas.BuildPipeline.Models;

    public class FakeDeploymentRepository : IDeploymentRepository
    {
        private static readonly IList<Deployment> _deployments =
            new List<Deployment>();

        public void AddDeployment(Deployment deployment)
        {
            if (deployment == null)
            {
                throw new ArgumentNullException("deployment");
            }

            int newId = 1;
            if (_deployments.Count > 0)
            {
                newId = _deployments.Max(d => d.Id.Value) + 1;
            }

            deployment.ChangeId(newId);
            _deployments.Add(deployment);
        }

        public IEnumerable<Deployment> GetByEnvironment(int environmentId)
        {
            throw new NotImplementedException();
        }

        public Deployment GetDeployment(int id)
        {
            return _deployments.SingleOrDefault(d => d.Id == id);
        }

        public IEnumerable<Deployment> GetDeployments(int pipelineId)
        {
            return _deployments.Where(d => d.PipelineId == pipelineId);
        }

        public IEnumerable<Deployment> GetDueDeployments()
        {
            return _deployments.Where(d => !d.HasRun);
        }

        public void UpdateDeployment(Deployment deployment)
        {
        }
    }
}