namespace Uncas.BuildPipeline.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;

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

        public int? Id { get; private set; }

        public DateTime Created { get; private set; }

        public int EnvironmentId { get; private set; }

        public int PipelineId { get; private set; }

        public DateTime? Started { get; private set; }

        public DateTime? Completed { get; private set; }

        public bool HasRun
        {
            get
            {
                return Started.HasValue &&
                       Completed.HasValue;
            }
        }

        public static Deployment Reconstruct(
            int id,
            DateTime created,
            int pipelineId,
            int environmentId,
            DateTime? started,
            DateTime? completed)
        {
            var deployment = new Deployment(
                pipelineId,
                environmentId);
            deployment.Id = id;
            deployment.Created = created;
            deployment.Started = started;
            deployment.Completed = completed;
            return deployment;
        }

        public void Complete()
        {
            if (!Started.HasValue)
            {
                throw new NotSupportedException(
                    "A deployment must be started before it can be completed.");
            }

            Completed = DateTime.Now;
        }

        public void Start()
        {
            Started = DateTime.Now;
        }

        internal void ChangeId(int newId)
        {
            Id = newId;
        }
    }
}