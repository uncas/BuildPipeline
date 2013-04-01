using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Uncas.BuildPipeline.ApplicationServices;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Utilities;
using Uncas.Core.Data;
using Environment = Uncas.BuildPipeline.Models.Environment;

namespace Uncas.BuildPipeline.Tests.Unit.ApplicationServices
{
    [TestFixture]
    public class DeploymentServiceTests : WithFixture<DeploymentService>
    {
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
                DateTime.Now,
                "x",
                "x");
            Mock<IEnvironmentRepository> environmentRepositoryMock = Fixture.FreezeMock<IEnvironmentRepository>();
            environmentRepositoryMock.Setup(
                er => er.GetEnvironment(environmentId)).Returns(
                    environment);
            Mock<IPipelineRepository> pipelineRepositoryMock = Fixture.FreezeMock<IPipelineRepository>();
            pipelineRepositoryMock.Setup(
                pr => pr.GetPipeline(pipelineId)).Returns(
                    pipeline);
        }

        private void WithDeployments(
            params Deployment[] deployments)
        {
            Mock<IDeploymentRepository> deploymentRepositoryMock = Fixture.FreezeMock<IDeploymentRepository>();
            deploymentRepositoryMock.Setup(
                dr => dr.GetDueDeployments(It.IsAny<PagingInfo>()))
                .Returns(new List<Deployment>(deployments));
        }

        [Test]
        public void DeployDueDeployments_NoDue_DeploysNone()
        {
            Mock<IDeploymentUtility> deploymentUtilityMock = Fixture.FreezeMock<IDeploymentUtility>();

            Sut.DeployDueDeployments();

            deploymentUtilityMock.Verify(
                du => du.Deploy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Environment>()),
                Times.Never());
        }

        [Test]
        public void DeployDueDeployments_WhenCalledSecondTime_DeploysNoneSecondTime()
        {
            Mock<IDeploymentUtility> deploymentUtilityMock = Fixture.FreezeMock<IDeploymentUtility>();
            const int pipelineId = 1;
            const int environmentId = 1;
            var deployment = new Deployment(
                pipelineId,
                environmentId);
            SetupRepositories(pipelineId, environmentId);
            WithDeployments(deployment);
            Sut.DeployDueDeployments();
            deploymentUtilityMock.Verify(
                du => du.Deploy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Environment>()),
                Times.Once());
            WithDeployments();

            Sut.DeployDueDeployments();

            deploymentUtilityMock.Verify(
                du => du.Deploy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Environment>()),
                Times.Once());
        }

        [Test]
        public void DeployDueDeployments_WhenContainsOne_DeploysOne()
        {
            Mock<IDeploymentUtility> deploymentUtilityMock = Fixture.FreezeMock<IDeploymentUtility>();
            const int pipelineId = 1;
            const int environmentId = 1;
            var deployment = new Deployment(
                pipelineId,
                environmentId);
            SetupRepositories(pipelineId, environmentId);
            WithDeployments(deployment);

            Sut.DeployDueDeployments();

            deploymentUtilityMock.Verify(
                du => du.Deploy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Environment>()),
                Times.Once());
        }

        [Test]
        public void Deploy_ValidCommand_IsDeployed()
        {
            const int pipelineId = 1;
            const int environmentId = 1;
            SetupRepositories(pipelineId, environmentId);

            Sut.Deploy(
                pipelineId,
                environmentId);

            // TODO: Assert that it is deployed alright.
        }

        [Test]
        public void GetDeployments_WhenScheduledNew_ContainsTheScheduled()
        {
            const int pipelineId = 1;
            const int environmentId = 1;
            SetupRepositories(pipelineId, environmentId);
            Mock<IDeploymentRepository> deploymentRepositoryMock = Fixture.FreezeMock<IDeploymentRepository>();
            var deployment = new Deployment(
                pipelineId,
                environmentId);
            deploymentRepositoryMock.Setup(
                dr => dr.GetDeployments(pipelineId))
                .Returns(new List<Deployment> {deployment});

            List<Deployment> scheduledDeployments =
                Sut.GetDeployments(
                    pipelineId).ToList();

            Assert.NotNull(scheduledDeployments);
            Assert.True(scheduledDeployments.Count > 0);
            Assert.True(scheduledDeployments.Any(d => d.PipelineId == pipelineId));
        }

        [Test]
        public void GetDueDeployments_WhenScheduledNew_ContainsTheScheduled()
        {
            const int pipelineId = 1;
            const int environmentId = 1;
            SetupRepositories(pipelineId, environmentId);
            var deployment = new Deployment(
                pipelineId,
                environmentId);
            WithDeployments(deployment);

            List<Deployment> scheduledDeployments =
                Sut.GetDueDeployments(null).ToList();

            Assert.NotNull(scheduledDeployments);
            Assert.True(scheduledDeployments.Count > 0);
            Assert.True(scheduledDeployments.Any(d => d.PipelineId == pipelineId));
        }
    }
}