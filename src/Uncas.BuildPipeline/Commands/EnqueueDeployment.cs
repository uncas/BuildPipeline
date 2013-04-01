namespace Uncas.BuildPipeline.Commands
{
    public class EnqueueDeployment : ICommand
    {
        public EnqueueDeployment(int pipelineId, int environmentId)
        {
            PipelineId = pipelineId;
            EnvironmentId = environmentId;
        }

        public int PipelineId { get; private set; }
        public int EnvironmentId { get; private set; }
    }
}