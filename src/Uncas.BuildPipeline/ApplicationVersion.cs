using System.Reflection;

namespace Uncas.BuildPipeline
{
    public static class ApplicationVersion
    {
        public static string GetVersion(Assembly assembly)
        {
            object[] customAttributes = assembly.GetCustomAttributes(false);
            return customAttributes.Maybe<AssemblyInformationalVersionAttribute, string>(
                x => x.InformationalVersion, () => assembly.GetName().Version.ToString());
        }
    }
}