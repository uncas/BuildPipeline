namespace Uncas.BuildPipeline.Tests.Integration.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using NUnit.Framework;
    using Uncas.BuildPipeline.Models;
    using Uncas.BuildPipeline.Repositories;

    [TestFixture]
    public class DeploymentRepositoryTests
    {
        #region Setup/Teardown

        [SetUp]
        public void BeforeEach()
        {
            // TODO: When a dedicated test database is in place, use the real repository here:
            deploymentRepository = new FakeDeploymentRepository();
        }

        #endregion

        private IDeploymentRepository deploymentRepository;

        [Test]
        public void AddDeployment_WhenAdded_CanBeRetrieved()
        {
            const int pipelineId = 1;
            const int environmentId = 1;
            var deployment = new Deployment(
                pipelineId,
                environmentId);
            deploymentRepository.AddDeployment(deployment);

            IEnumerable<Deployment> result =
                deploymentRepository.GetDeployments(pipelineId);

            Assert.True(result.Count() >= 1);
        }

        [Test]
        public void UpdateDeployment_WhenStarted_StartedIsUpdated()
        {
            // Arrange:
            var added = new Deployment(1, 1);
            deploymentRepository.AddDeployment(added);
            Thread.Sleep(10);
            added.Start();
            int deploymentId = added.Id.Value;

            // Act:
            deploymentRepository.UpdateDeployment(added);

            // Assert:
            Deployment updated = deploymentRepository.GetDeployment(deploymentId);
            Assert.NotNull(updated.Started);
            Assert.True(updated.Started > added.Created);
        }
    }
}