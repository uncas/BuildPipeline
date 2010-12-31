namespace Uncas.BuildPipeline.ApplicationServices
{
    public interface IDeploymentService
    {
        void Deploy(int pipelineId, int environmentId);
    }
}