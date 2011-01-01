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

    [TestFixture]
    public class DeploymentServiceTests
    {
        private IDeploymentService deploymentService;

        private Mock<IDeploymentRepository> deploymentRepositoryMock;
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
            this.deploymentRepositoryMock =
                new Mock<IDeploymentRepository>();
            this.deploymentService = new DeploymentService(
                this.environmentRepositoryMock.Object,
                this.pipelineRepositoryMock.Object,
                this.deploymentRepositoryMock.Object,
                this.deploymentUtilityMock.Object);
        }

        [Test]
        public void Deploy_ValidCommand_IsDeployed()
        {
            // Arrange:
            int pipelineId = 1;
            int environmentId = 1;
            SetupRepositories(pipelineId, environmentId);

            // Act:
            this.deploymentService.Deploy(
                pipelineId,
                environmentId);

            // TODO: Assert that it is deployed alright.
        }

        [Test]
        public void ScheduleDeployment_NonExistingInput_IsNotScheduled()
        {
            // Arrange:
            int pipelineId = 1;
            int environmentId = 1;
            DateTime scheduledStart = DateTime.Now.AddMinutes(5d);

            // Act:
            ScheduleDeploymentResult result =
                this.deploymentService.ScheduleDeployment(
                pipelineId,
                environmentId,
                scheduledStart);

            // Assert:
            Assert.False(result.Success);
            Assert.Null(result.Deployment);
            Assert.AreEqual(2, result.Errors.Count());
            this.deploymentRepositoryMock.Verify(
                dr => dr.AddDeployment(It.IsAny<Deployment>()),
                Times.Never());
        }

        [Test]
        public void ScheduleDeployment_ValidCommand_IsScheduled()
        {
            // Arrange:
            int pipelineId = 1;
            int environmentId = 1;
            DateTime scheduledStart = DateTime.Now.AddMinutes(5d);
            SetupRepositories(pipelineId, environmentId);

            // Act:
            ScheduleDeploymentResult result =
                this.deploymentService.ScheduleDeployment(
                pipelineId,
                environmentId,
                scheduledStart);

            // Assert:
            Assert.True(result.Success);
            Assert.NotNull(result.Deployment);
            Assert.AreEqual(pipelineId, result.Deployment.PipelineId);
            Assert.AreEqual(environmentId, result.Deployment.EnvironmentId);
            Assert.AreEqual(scheduledStart, result.Deployment.ScheduledStart);
            this.deploymentRepositoryMock.Verify(
                dr => dr.AddDeployment(It.IsAny<Deployment>()),
                Times.Once());
        }

        [Test]
        public void GetDeployments_WhenScheduledNew_ContainsTheScheduled()
        {
            // Arrange:
            int pipelineId = 1;
            int environmentId = 1;
            DateTime scheduledStart = DateTime.Now.AddMinutes(5d);
            SetupRepositories(pipelineId, environmentId);
            ScheduleDeploymentResult result =
                this.deploymentService.ScheduleDeployment(
                pipelineId,
                environmentId,
                scheduledStart);
            var deployment = new Deployment(
                pipelineId,
                environmentId,
                scheduledStart);
            this.deploymentRepositoryMock.Setup(
                dr => dr.GetDeployments(pipelineId)).
                Returns(new List<Deployment> { deployment });

            // Act:
            IEnumerable<Deployment> scheduledDeployments =
                this.deploymentService.GetDeployments(
                pipelineId);

            // Assert:
            Assert.NotNull(scheduledDeployments);
            Assert.True(scheduledDeployments.Count() > 0);
            Assert.True(scheduledDeployments.Any(d => d.ScheduledStart == scheduledStart));
        }

        [Test]
        public void GetDueDeployments_WhenScheduledNew_ContainsTheScheduled()
        {
            // Arrange:
            int pipelineId = 1;
            int environmentId = 1;
            DateTime scheduledStart = DateTime.Now.AddMinutes(5d);
            SetupRepositories(pipelineId, environmentId);
            ScheduleDeploymentResult result =
                this.deploymentService.ScheduleDeployment(
                pipelineId,
                environmentId,
                scheduledStart);
            var deployment = new Deployment(
                pipelineId,
                environmentId,
                scheduledStart);
            this.deploymentRepositoryMock.Setup(
                dr => dr.GetDueDeployments()).
                Returns(new List<Deployment> { deployment });

            // Act:
            IEnumerable<Deployment> scheduledDeployments =
                this.deploymentService.GetDueDeployments();

            // Assert:
            Assert.NotNull(scheduledDeployments);
            Assert.True(scheduledDeployments.Count() > 0);
            Assert.True(scheduledDeployments.Any(d => d.ScheduledStart == scheduledStart));
        }

        [Test]
        public void DeployDueDeployments_NoDue_DeploysNone()
        {
            // Act:
            this.deploymentService.DeployDueDeployments();

            // Assert:
            this.deploymentUtilityMock.Verify(
                du => du.Deploy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<BuildPipeline.Models.Environment>()),
                Times.Never());
        }

        [Test]
        public void DeployDueDeployments_WhenContainsOne_DeploysOne()
        {
            // Arrange:
            int pipelineId = 1;
            int environmentId = 1;
            DateTime scheduledStart = DateTime.Now;
            var deployment = new Deployment(
                pipelineId,
                environmentId,
                scheduledStart);
            SetupRepositories(pipelineId, environmentId);
            this.deploymentRepositoryMock.Setup(
                dr => dr.GetDueDeployments()).
                Returns(new List<Deployment> { deployment });

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