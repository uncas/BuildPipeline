namespace Uncas.BuildPipeline.WindowsService
{
    using Uncas.BuildPipeline.ApplicationServices;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.BuildPipeline.Utilities;

    internal static class Bootstrapper
    {
        public static IDeploymentService GetDeploymentService()
        {
            // TODO: Use real deployment repository here:
            return new DeploymentService(
                new EnvironmentRepository(),
                new PipelineRepository(),
                new FakeDeploymentRepository(),
                new DeploymentUtility());
        }
    }
}
