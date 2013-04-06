using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.Utilities
{
    public interface IDeploymentUtility
    {
        void Deploy(
            string packagePath,
            Environment environment,
            string customScript);
    }
}