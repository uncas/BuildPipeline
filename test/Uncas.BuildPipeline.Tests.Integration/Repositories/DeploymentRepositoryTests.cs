namespace Uncas.BuildPipeline.Tests.Integration.Repositories
{
    using System.Linq;
    using System.Threading;
    using NUnit.Framework;
    using Uncas.BuildPipeline.Models;
    using Uncas.BuildPipeline.Repositories;

    [TestFixture]
    public class DeploymentRepositoryTests
    {
        private IDeploymentRepository deploymentRepository;

        [SetUp]
        public void BeforeEach()
        {
            this.deploymentRepository = new FakeDeploymentRepository();
        }

        [Test]
        public void AddDeployment_WhenAdded_CanBeRetrieved()
        {
            const int pipelineId = 1;
            const int environmentId = 1;
            var deployment = new Deployment(
                pipelineId,
                environmentId);
            this.deploymentRepository.AddDeployment(deployment);

            var result =
                this.deploymentRepository.GetDeployments(pipelineId);

            Assert.True(result.Count() >= 1);
        }

        [Test]
        public void UpdateDeployment_WhenStarted_StartedIsUpdated()
        {
            // Arrange:
            var added = new Deployment(1, 1);
            this.deploymentRepository.AddDeployment(added);
            Thread.Sleep(10);
            added.Start();
            int deploymentId = added.Id.Value;

            // Act:
            this.deploymentRepository.UpdateDeployment(added);

            // Assert:
            var updated = this.deploymentRepository.GetDeployment(deploymentId);
            Assert.NotNull(updated.Started);
            Assert.True(updated.Started > added.Created);
        }
    }
}