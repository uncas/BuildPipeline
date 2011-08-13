namespace Uncas.BuildPipeline.Tests.Integration.ApplicationServices
{
    using System;
    using Moq;
    using NUnit.Framework;
    using Uncas.BuildPipeline.ApplicationServices;
    using Uncas.BuildPipeline.Models;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.BuildPipeline.Utilities;
    using Environment = Uncas.BuildPipeline.Models.Environment;

    [TestFixture]
    public class DeploymentServiceTests
    {
        #region Setup/Teardown

        [SetUp]
        public void BeforeEach()
        {
            environmentRepositoryMock =
                new Mock<IEnvironmentRepository>();
            pipelineRepositoryMock =
                new Mock<IPipelineRepository>();
            deploymentUtilityMock =
                new Mock<IDeploymentUtility>();
            deploymentRepository = new FakeDeploymentRepository();
            deploymentService = new DeploymentService(
                environmentRepositoryMock.Object,
                pipelineRepositoryMock.Object,
                deploymentRepository,
                deploymentUtilityMock.Object);
        }

        #endregion

        private IDeploymentService deploymentService;

        private IDeploymentRepository deploymentRepository;

        private Mock<IDeploymentUtility> deploymentUtilityMock;
        private Mock<IEnvironmentRepository> environmentRepositoryMock;
        private Mock<IPipelineRepository> pipelineRepositoryMock;

        private void SetupRepositories(
            int pipelineId,
            int environmentId)
        {
            var environment = new Environment();
            var pipeline = new Pipeline(
                pipelineId,
                "A",
                1,
                "x",
                "x",
                DateTime.Now,
                "x",
                "x");
            environmentRepositoryMock.Setup(
                er => er.GetEnvironment(environmentId)).Returns(
                    environment);
            pipelineRepositoryMock.Setup(
                pr => pr.GetPipeline(pipelineId)).Returns(
                    pipeline);
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
            deploymentRepository.AddDeployment(deployment);
            deploymentService.DeployDueDeployments();
            deploymentUtilityMock.Verify(
                du => du.Deploy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Environment>()),
                Times.Once());

            // Act:
            deploymentService.DeployDueDeployments();

            // Assert:
            deploymentUtilityMock.Verify(
                du => du.Deploy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Environment>()),
                Times.Once());
        }
    }
}