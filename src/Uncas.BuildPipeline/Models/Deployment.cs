namespace Uncas.BuildPipeline.Models
{
    using System;

    public class Deployment
    {
        public Deployment(
            int pipelineId,
            int environmentId,
            DateTime scheduledStart)
        {
            this.PipelineId = pipelineId;
            this.EnvironmentId = environmentId;
            this.ScheduledStart = scheduledStart;
        }

        public int EnvironmentId { get; private set; }
        public int PipelineId { get; private set; }
        public DateTime ScheduledStart { get; private set; }
    }
}