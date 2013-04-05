using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Tests.Unit.Utilities
{
    public class DeploymentUtilityTests : WithFixture<DeploymentUtility>
    {
        [Test]
        public void Deploy()
        {
            Sut.Deploy(
                Fixture.Create<string>(),
                Fixture.Create<string>(),
                Fixture.Create<Environment>(),
                Fixture.Create<string>());
        }
    }
}