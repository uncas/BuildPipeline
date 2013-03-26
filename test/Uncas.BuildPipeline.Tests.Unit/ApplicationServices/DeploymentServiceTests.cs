namespace Uncas.BuildPipeline.Tests.Unit.ApplicationServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using Uncas.BuildPipeline.ApplicationServices;
    using Uncas.BuildPipeline.ApplicationServices.Results;
    using Uncas.BuildPipeline.Models;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.BuildPipeline.Utilities;
    using Uncas.Core.Data;
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
            deploymentRepositoryMock =
                new Mock<IDeploymentRepository>();
            deploymentService = new DeploymentService(
                environmentRepositoryMock.Object,
                pipelineRepositoryMock.Object,
                deploymentRepositoryMock.Object,
                deploymentUtilityMock.Object);
        }

        #endregion

        private IDeploymentService deploymentService;

        private Mock<IDeploymentRepository> deploymentRepositoryMock;
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
                "1",
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

        private void WithDeployments(params Deployment[] deployments)
        {
            deploymentRepositoryMock.Setup(
                dr => dr.GetDueDeployments(It.IsAny<PagingInfo>()))
                .Returns(new List<Deployment>(deployments));
        }

        [Test]
        public void DeployDueDeployments_NoDue_DeploysNone()
        {
            // Act:
            deploymentService.DeployDueDeployments();

            // Assert:
            deploymentUtilityMock.Verify(
                du => du.Deploy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Environment>()),
                Times.Never());
        }

        [Test]
        public void DeployDueDeployments_WhenCalledSecondTime_DeploysNoneSecondTime()
        {
            // Arrange:
            const int pipelineId = 1;
            const int environmentId = 1;
            var deployment = new Deployment(
                pipelineId,
                environmentId);
            SetupRepositories(pipelineId, environmentId);
            WithDeployments(deployment);
            deploymentService.DeployDueDeployments();
            deploymentUtilityMock.Verify(
                du => du.Deploy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Environment>()),
                Times.Once());
            WithDeployments();

            // Act:
            deploymentService.DeployDueDeployments();

            // Assert:
            deploymentUtilityMock.Verify(
                du => du.Deploy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Environment>()),
                Times.Once());
        }

        [Test]
        public void DeployDueDeployments_WhenContainsOne_DeploysOne()
        {
            // Arrange:
            const int pipelineId = 1;
            const int environmentId = 1;
            var deployment = new Deployment(
                pipelineId,
                environmentId);
            SetupRepositories(pipelineId, environmentId);
            WithDeployments(deployment);

            // Act:
            deploymentService.DeployDueDeployments();

            // Assert:
            deploymentUtilityMock.Verify(
                du => du.Deploy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Environment>()),
                Times.Once());
        }

        [Test]
        public void Deploy_ValidCommand_IsDeployed()
        {
            // Arrange:
            const int pipelineId = 1;
            const int environmentId = 1;
            SetupRepositories(pipelineId, environmentId);

            // Act:
            deploymentService.Deploy(
                pipelineId,
                environmentId);

            // TODO: Assert that it is deployed alright.
        }

        [Test]
        public void GetDeployments_WhenScheduledNew_ContainsTheScheduled()
        {
            // Arrange:
            const int pipelineId = 1;
            const int environmentId = 1;
            SetupRepositories(pipelineId, environmentId);
            deploymentService.ScheduleDeployment(
                pipelineId,
                environmentId);
            var deployment = new Deployment(
                pipelineId,
                environmentId);
            deploymentRepositoryMock.Setup(
                dr => dr.GetDeployments(pipelineId))
                .Returns(new List<Deployment> { deployment });

            // Act:
            IEnumerable<Deployment> scheduledDeployments =
                deploymentService.GetDeployments(
                    pipelineId);

            // Assert:
            Assert.NotNull(scheduledDeployments);
            Assert.True(scheduledDeployments.Count() > 0);
            Assert.True(scheduledDeployments.Any(d => d.PipelineId == pipelineId));
        }

        [Test]
        public void GetDueDeployments_WhenScheduledNew_ContainsTheScheduled()
        {
            // Arrange:
            const int pipelineId = 1;
            const int environmentId = 1;
            SetupRepositories(pipelineId, environmentId);
            deploymentService.ScheduleDeployment(
                pipelineId,
                environmentId);
            var deployment = new Deployment(
                pipelineId,
                environmentId);
            WithDeployments(deployment);

            // Act:
            IEnumerable<Deployment> scheduledDeployments =
                deploymentService.GetDueDeployments(null);

            // Assert:
            Assert.NotNull(scheduledDeployments);
            Assert.True(scheduledDeployments.Count() > 0);
            Assert.True(scheduledDeployments.Any(d => d.PipelineId == pipelineId));
        }

        [Test]
        public void ScheduleDeployment_NonExistingInput_IsNotScheduled()
        {
            // Arrange:
            const int pipelineId = 1;
            const int environmentId = 1;

            // Act:
            ScheduleDeploymentResult result =
                deploymentService.ScheduleDeployment(
                    pipelineId,
                    environmentId);

            // Assert:
            Assert.False(result.Success);
            Assert.Null(result.Deployment);
            Assert.AreEqual(2, result.Errors.Count());
            deploymentRepositoryMock.Verify(
                dr => dr.AddDeployment(It.IsAny<Deployment>()),
                Times.Never());
        }

        [Test]
        public void ScheduleDeployment_ValidCommand_IsScheduled()
        {
            // Arrange:
            const int pipelineId = 1;
            const int environmentId = 1;
            SetupRepositories(pipelineId, environmentId);

            // Act:
            ScheduleDeploymentResult result =
                deploymentService.ScheduleDeployment(
                    pipelineId,
                    environmentId);

            // Assert:
            Assert.True(result.Success);
            Assert.NotNull(result.Deployment);
            Assert.AreEqual(pipelineId, result.Deployment.PipelineId);
            Assert.AreEqual(environmentId, result.Deployment.EnvironmentId);
            deploymentRepositoryMock.Verify(
                dr => dr.AddDeployment(It.IsAny<Deployment>()),
                Times.Once());
        }
    }
}