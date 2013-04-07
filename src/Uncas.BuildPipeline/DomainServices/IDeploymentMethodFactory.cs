using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.DomainServices
{
    public interface IDeploymentMethodFactory
    {
        IDeploymentMethod CreateDeploymentMethod(
            ProjectReadModel project,
            Environment environment);
    }
}