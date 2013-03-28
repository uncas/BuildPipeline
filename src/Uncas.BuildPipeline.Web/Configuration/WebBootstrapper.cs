using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using UnityConfiguration;

namespace Uncas.BuildPipeline.Web.Configuration
{
    public static class WebBootstrapper
    {
        private static readonly IUnityContainer Container = BuildUnityContainer();

        public static void InitialiseForWeb()
        {
            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static T TryResolve<T>()
        {
            if (Container.IsRegistered<T>())
                return Container.Resolve<T>();
            return default(T);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.Configure(x => x.AddRegistry<SharedRegistry>());
            container.Configure(x => x.AddRegistry<WebRegistry>());
            return container;
        }
    }
}