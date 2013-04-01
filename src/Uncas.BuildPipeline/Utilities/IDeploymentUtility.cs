using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.Utilities
{
    public interface IDeploymentUtility
    {
        void Deploy(
            string packagePath,
            string workingDirectory,
            Environment environment,
            string customScript);
    }
}