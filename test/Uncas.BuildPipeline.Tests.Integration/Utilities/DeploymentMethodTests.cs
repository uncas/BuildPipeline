using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.DomainServices;
using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.Tests.Integration.Utilities
{
    [TestFixture]
    public class DeploymentMethodTests : WithBootstrapping<IDeploymentMethod>
    {
        [Test]
        [Ignore]
        public void Deploy_ExistingPackage_IsDeployedAlright()
        {
            const string packagePath =
                @"C:\Builds\BuildPipeline\Artifacts\Unit\packages\Uncas.BuildPipeline-0.1.35.966.zip";

            Sut.Deploy(packagePath, Fixture.Create<Environment>(),
                       Fixture.Create<ProjectReadModel>());
        }
    }
}