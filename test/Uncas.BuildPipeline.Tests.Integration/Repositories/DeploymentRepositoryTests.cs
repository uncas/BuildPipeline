using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;

namespace Uncas.BuildPipeline.Tests.Integration.Repositories
{
    [TestFixture]
    public class DeploymentRepositoryTests : WithBootstrapping<IDeploymentRepository>
    {
        [Test]
        public void AddDeployment_WhenAdded_CanBeRetrieved()
        {
            const int pipelineId = 1;
            const int environmentId = 1;
            var deployment = new Deployment(pipelineId, environmentId);
            Sut.AddDeployment(deployment);

            IEnumerable<Deployment> result = Sut.GetDeployments(pipelineId);

            Assert.True(result.Any());
        }

        [Test]
        public void GetByEnvironment_X()
        {
            Sut.GetByEnvironment(1);
        }

        [Test]
        public void UpdateDeployment_WhenStarted_StartedIsUpdated()
        {
            // Arrange:
            var added = new Deployment(1, 1);
            Sut.AddDeployment(added);
            Thread.Sleep(10);
            added.Start();
            int deploymentId = added.Id.Value;

            // Act:
            Sut.UpdateDeployment(added);

            // Assert:
            Deployment updated = Sut.GetDeployment(deploymentId);
            Assert.NotNull(updated.Started);
            Assert.True(updated.Started > added.Created);
        }
    }
}