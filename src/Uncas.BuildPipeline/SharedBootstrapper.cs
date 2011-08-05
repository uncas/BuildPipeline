namespace Uncas.BuildPipeline
{
    using System;
    using System.Collections.Generic;
    using Uncas.BuildPipeline.ApplicationServices;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.BuildPipeline.Utilities;

    public static class SharedBootstrapper
    {
        private static Dictionary<Type, Func<object>> _registered;

        private static Dictionary<Type, Func<object>> Registered
        {
            get
            {
                if (_registered == null)
                {
                    _registered = new Dictionary<Type, Func<object>>();
                    _registered.Add(
                        typeof(IDeploymentService),
                        () => new DeploymentService(
                                  Resolve<IEnvironmentRepository>(),
                                  Resolve<IPipelineRepository>(),
                                  Resolve<IDeploymentRepository>(),
                                  new DeploymentUtility()));
                    _registered.Add(
                        typeof(IEnvironmentRepository),
                        () => new EnvironmentRepository());
                    _registered.Add(
                        typeof(IPipelineRepository),
                        () => new PipelineRepository(
                                  Resolve<IBuildPipelineRepositoryConfiguration>()));
                    _registered.Add(
                        typeof(IBuildPipelineRepositoryConfiguration),
                        () => new BuildPipelineRepositoryConfiguration());
                    _registered.Add(
                        typeof(IDeploymentRepository),
                        () => new DeploymentRepository(
                                  Resolve<IBuildPipelineRepositoryConfiguration>()));
                }

                return _registered;
            }
        }

        public static T Resolve<T>() where T : class
        {
            Type interfaceType = typeof(T);
            if (!Registered.ContainsKey(interfaceType))
            {
                return null;
            }

            return Registered[interfaceType]() as T;
        }
    }
}