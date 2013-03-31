using Moq;
using NUnit.Framework;
using Uncas.BuildPipeline.ApplicationServices;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Tests.Unit;
using Uncas.Core.Data;

namespace Uncas.BuildPipeline.Tests.Integration.ApplicationServices
{
    [TestFixture]
    public class DeploymentServiceTests : WithFixture<DeploymentService>
    {
        [Test]
        public void DeployDueDeployments_WhenCalled_ReadsDueDeployments()
        {
            Mock<IDeploymentRepository> deploymentRepositoryMock = Fixture.FreezeMock<IDeploymentRepository>();

            Sut.DeployDueDeployments();

            deploymentRepositoryMock.Verify(
                du => du.GetDueDeployments(It.IsAny<PagingInfo>()),
                Times.Once());
        }
    }
}