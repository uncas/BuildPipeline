namespace Uncas.BuildPipeline.Utilities
{
    using Uncas.BuildPipeline.Models;

    public interface IDeploymentUtility
    {
        void Deploy(
            string packagePath, 
            string workingDirectory, 
            Environment environment);
    }
}