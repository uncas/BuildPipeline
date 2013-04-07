using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.DomainServices;
using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.Tests.Integration.DomainServices
{
    public class DeploymentMethodFactoryTests : WithBootstrapping<DeploymentMethodFactory>
    {
        [Test]
        public void CreateDeploymentMethod_Default_Powershell()
        {
            Assert.IsInstanceOf<PowershellDeployment>(Sut.CreateDeploymentMethod(
                Fixture.Create<ProjectReadModel>(), Fixture.Create<Environment>()));
        }
    }
}