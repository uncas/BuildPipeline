namespace Uncas.BuildPipeline.Repositories
{
    using Uncas.BuildPipeline.Models;

    public interface IDeploymentRepository
    {
        void AddDeployment(Deployment deployment);
    }
}
