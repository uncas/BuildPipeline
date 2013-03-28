using Microsoft.Practices.Unity;
using UnityConfiguration;

namespace Uncas.BuildPipeline.WindowsService
{
    public static class Bootstrapper
    {
        private static IUnityContainer _container;

        public static void Initialize()
        {
            _container = BuildUnityContainer();
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
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