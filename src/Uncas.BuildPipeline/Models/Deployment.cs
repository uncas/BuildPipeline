namespace Uncas.BuildPipeline.Models
{
    public class Deployment
    {
        public Deployment(
            int pipelineId,
            int environmentId)
        {
            this.PipelineId = pipelineId;
            this.EnvironmentId = environmentId;
        }

        public int EnvironmentId { get; private set; }
        public int PipelineId { get; private set; }
    }
}