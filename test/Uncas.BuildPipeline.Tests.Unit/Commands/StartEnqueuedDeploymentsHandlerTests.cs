using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Uncas.BuildPipeline.Commands;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Utilities;
using Uncas.Core.Data;
using Environment = Uncas.BuildPipeline.Models.Environment;

namespace Uncas.BuildPipeline.Tests.Unit.Commands
{
    [TestFixture]
    public class StartEnqueuedDeploymentsHandlerTests : WithFixture<StartEnqueuedDeploymentsHandler>
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

            Sut.Handle(new StartEnqueuedDeployments());

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
            Sut.Handle(new StartEnqueuedDeployments());
            deploymentUtilityMock.Verify(
                du => du.Deploy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Environment>()),
                Times.Once());
            WithDeployments();

            Sut.Handle(new StartEnqueuedDeployments());

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

            Sut.Handle(new StartEnqueuedDeployments());

            deploymentUtilityMock.Verify(
                du => du.Deploy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Environment>()),
                Times.Once());
        }
    }
}