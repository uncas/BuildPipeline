namespace Uncas.BuildPipeline.Web
{
    public static class Bootstrapper
    {
        public static T Resolve<T>() where T : class
        {
            return SharedBootstrapper.Resolve<T>();
        }
    }
}