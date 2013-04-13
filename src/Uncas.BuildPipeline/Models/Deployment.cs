using System;
using System.Diagnostics.CodeAnalysis;

namespace Uncas.BuildPipeline.Models
{
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1724:TypeNamesShouldNotMatchNamespaces",
        Justification = "But it *is* a deployment!")]
    public class Deployment
    {
        private Deployment()
        {
        }

        public Deployment(
            int pipelineId,
            int environmentId)
        {
            PipelineId = pipelineId;
            EnvironmentId = environmentId;
            Created = DateTime.Now;
        }

        public int? DeploymentId { get; private set; }
        public int EnvironmentId { get; private set; }
        public int PipelineId { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Started { get; private set; }
        public DateTime? Completed { get; private set; }

        public void MarkAsCompleted()
        {
            if (!Started.HasValue)
            {
                throw new InvalidOperationException(
                    "A deployment must be started before it can be completed.");
            }

            Completed = DateTime.Now;
        }

        public void MarkAsStarted()
        {
            Started = DateTime.Now;
        }

        internal void ChangeId(int newId)
        {
            DeploymentId = newId;
        }
    }
}