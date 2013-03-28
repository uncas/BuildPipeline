using ServiceStack.Configuration;

namespace Uncas.BuildPipeline.Web.Configuration
{
    public class UnityContainerAdapter : IContainerAdapter
    {
        #region IContainerAdapter Members

        public T TryResolve<T>()
        {
            return WebBootstrapper.TryResolve<T>();
        }

        public T Resolve<T>()
        {
            return WebBootstrapper.Resolve<T>();
        }

        #endregion
    }
}