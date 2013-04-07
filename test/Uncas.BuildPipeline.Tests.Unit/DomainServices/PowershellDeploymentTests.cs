using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.DomainServices;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Tests.Unit.DomainServices
{
    public class PowershellDeploymentTests : WithFixture<PowershellDeployment>
    {
        [Test]
        public void Deploy_MissingPowershellScript_RunsNothing()
        {
            Mock<IPowershellUtility> powershellUtilityMock =
                Fixture.FreezeMock<IPowershellUtility>();
            Fixture.Inject(string.Empty);

            Sut.Deploy(
                Fixture.Create<string>(),
                Fixture.Create<Environment>(),
                Fixture.Create<ProjectReadModel>());

            powershellUtilityMock.Verify(
                x => x.RunPowershell(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never());
        }

        [Test]
        public void Deploy_ValidInput_RunsPowershell()
        {
            Mock<IPowershellUtility> powershellUtilityMock =
                Fixture.FreezeMock<IPowershellUtility>();

            Sut.Deploy(
                Fixture.Create<string>(),
                Fixture.Create<Environment>(),
                Fixture.Create<ProjectReadModel>());

            powershellUtilityMock.Verify(
                x => x.RunPowershell(It.IsAny<string>(), It.IsAny<string>()),
                Times.Once());
        }
    }
}