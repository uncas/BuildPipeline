namespace Uncas.BuildPipeline.Models
{
    using System;

    public class Deployment
    {
        public Deployment(
            int pipelineId,
            int environmentId)
        {
            this.PipelineId = pipelineId;
            this.EnvironmentId = environmentId;
            this.Created = DateTime.Now;
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

        public void Start()
        {
            this.Started = DateTime.Now;
        }

        public void Complete()
        {
            if (!this.Started.HasValue)
            {
                throw new NotSupportedException(
                    "A deployment must be started before it can be completed.");
            }

            this.Completed = DateTime.Now;
        }

        internal void ChangeId(int newId)
        {
            this.Id = newId;
        }
    }
}