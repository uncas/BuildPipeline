using Microsoft.Practices.Unity;
using UnityConfiguration;

namespace Uncas.BuildPipeline.Tests.Integration
{
    public static class TestBootstrapper
    {
        private static readonly IUnityContainer Container = BuildUnityContainer();

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.Configure(x => x.AddRegistry<SharedRegistry>());
            return container;
        }
    }
}