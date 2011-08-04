namespace Uncas.BuildPipeline.Web
{
    using Uncas.BuildPipeline.ApplicationServices;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.BuildPipeline.Utilities;

    public static class Bootstrapper
    {
        public static T Resolve<T>() where T : class
        {
            if (typeof(T) == typeof(IDeploymentService))
            {
                return new DeploymentService(
                    Resolve<IEnvironmentRepository>(),
                    Resolve<IPipelineRepository>(),
                    new DeploymentRepository(),
                    new DeploymentUtility()) as T;
            }
            else if (typeof(T) == typeof(IEnvironmentRepository))
            {
                return new EnvironmentRepository() as T;
            }
            else if (typeof(T) == typeof(IPipelineRepository))
            {
                return new PipelineRepository() as T;
            }

            return null;
        }
    }
}