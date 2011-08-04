namespace Uncas.BuildPipeline.Web
{
    using Uncas.BuildPipeline.ApplicationServices;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.BuildPipeline.Utilities;

    public static class Bootstrapper
    {
        public static IDeploymentService GetDeploymentService()
        {
            return new DeploymentService(
                GetEnvironmentRepository(),
                GetPipelineRepository(),
                new DeploymentRepository(),
                new DeploymentUtility());
        }

        public static IEnvironmentRepository GetEnvironmentRepository()
        {
            return new EnvironmentRepository();
        }

        public static IPipelineRepository GetPipelineRepository()
        {
            return new PipelineRepository();
        }
    }
}