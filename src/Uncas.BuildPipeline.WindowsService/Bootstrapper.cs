using Microsoft.Practices.Unity;
using UnityConfiguration;

namespace Uncas.BuildPipeline.WindowsService
{
    public static class Bootstrapper
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
            container.Configure(x => x.AddRegistry<ServiceRegistry>());
            return container;
        }
    }
}