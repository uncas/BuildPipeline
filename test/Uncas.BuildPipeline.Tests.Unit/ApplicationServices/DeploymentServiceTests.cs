﻿namespace Uncas.BuildPipeline.Tests.Unit.ApplicationServices
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
        private Mock<IEnvironmentRepository> environmentRepositoryMock;
        private Mock<IPipelineRepository> pipelineRepositoryMock;

        [SetUp]
        public void BeforeEach()
        {
            this.environmentRepositoryMock =
                new Mock<IEnvironmentRepository>();
            this.pipelineRepositoryMock =
                new Mock<IPipelineRepository>();
            var deploymentUtilityMock =
                new Mock<IDeploymentUtility>();
            this.deploymentService = new DeploymentService(
                this.environmentRepositoryMock.Object,
                this.pipelineRepositoryMock.Object,
                deploymentUtilityMock.Object);
        }

        [Test]
        public void Deploy_ValidCommand_IsDeployed()
        {
            // Arrange:
            int pipelineId = 1;
            int environmentId = 1;
            var environment = new Models.Environment();
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

            // Act:
            this.deploymentService.Deploy(
                pipelineId,
                environmentId);

            // TODO: Assert that it is deployed alright.
        }

        /*[Test]
        public void ScheduleDeployment_ValidCommand_IsScheduled()
        {
            int pipelineId = 1;
            int environmentId = 1;
            DateTime scheduledStart = DateTime.Now.AddMinutes(5d);

            ScheduleDeploymentResult result = 
                this.deploymentService.ScheduleDeployment(
                pipelineId,
                environmentId,
                scheduledStart);

        }*/

        /*[Test]
        public void GetDeployments_WhenScheduledNew_ContainsTheScheduled()
        {
            IEnumerable<Deployment> scheduledDeployments =
                this.deploymentService.GetDeployments(
                pipelineId);

            Assert.NotNull(scheduledDeployments);
            Assert.True(scheduledDeployments.Count() > 0);
            Assert.True(scheduledDeployments.Any(sd => sd.ScheduledStart = scheduledStart));
        }*/
    }
}
