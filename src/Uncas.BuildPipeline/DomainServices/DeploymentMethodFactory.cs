using Microsoft.Practices.ServiceLocation;
using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.DomainServices
{
    public class DeploymentMethodFactory : IDeploymentMethodFactory
    {
        private readonly IServiceLocator _container;

        public DeploymentMethodFactory(IServiceLocator locator)
        {
            _container = locator;
        }

        #region IDeploymentMethodFactory Members

        public IDeploymentMethod CreateDeploymentMethod(
            ProjectReadModel project,
            Environment environment)
        {
            return _container.GetInstance<PowershellDeployment>();
        }

        #endregion
    }
}