using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.DomainServices
{
    public interface IDeploymentMethod
    {
        void Deploy(
            string packagePath,
            Environment environment,
            ProjectReadModel project);
    }
}