namespace Uncas.BuildPipeline.Tests.Integration.ApplicationServices
{
    using System;
    using Moq;
    using NUnit.Framework;
    using Uncas.BuildPipeline.ApplicationServices;
    using Uncas.BuildPipeline.Models;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.BuildPipeline.Utilities;

    [TestFixture]
    public class DeploymentServiceTests
    {
        private IDeploymentService deploymentService;

        private IDeploymentRepository deploymentRepository;

        private Mock<IDeploymentUtility> deploymentUtilityMock;
        private Mock<IEnvironmentRepository> environmentRepositoryMock;
        private Mock<IPipelineRepository> pipelineRepositoryMock;

        [SetUp]
        public void BeforeEach()
        {
            this.environmentRepositoryMock =
                new Mock<IEnvironmentRepository>();
            this.pipelineRepositoryMock =
                new Mock<IPipelineRepository>();
            this.deploymentUtilityMock =
                new Mock<IDeploymentUtility>();
            this.deploymentRepository = new FakeDeploymentRepository();
            this.deploymentService = new DeploymentService(
                this.environmentRepositoryMock.Object,
                this.pipelineRepositoryMock.Object,
                this.deploymentRepository,
                this.deploymentUtilityMock.Object);
        }

        [Test]
        public void DeployDueDeployments_WhenCalledSecondTime_DeploysNoneSecondTime()
        {
            // Arrange:
            int pipelineId = 1;
            int environmentId = 1;
            var deployment = new Deployment(
                pipelineId,
                environmentId);
            SetupRepositories(pipelineId, environmentId);
            this.deploymentRepository.AddDeployment(deployment);
            this.deploymentService.DeployDueDeployments();
            this.deploymentUtilityMock.Verify(
                du => du.Deploy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<BuildPipeline.Models.Environment>()),
                Times.Once());

            // Act:
            this.deploymentService.DeployDueDeployments();

            // Assert:
            this.deploymentUtilityMock.Verify(
                du => du.Deploy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<BuildPipeline.Models.Environment>()),
                Times.Once());
        }

        private void SetupRepositories(
            int pipelineId,
            int environmentId)
        {
            var environment = new Uncas.BuildPipeline.Models.Environment();
            var pipeline = new Pipeline(
                pipelineId,
                "A",
                1,
                "x",
                "x",
                DateTime.Now,
                "x",
                "x");
            this.environmentRepositoryMock.Setup(
                er => er.GetEnvironment(environmentId)).Returns(
                environment);
            this.pipelineRepositoryMock.Setup(
                pr => pr.GetPipeline(pipelineId)).Returns(
                pipeline);
        }
    }
}