using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Commands;
using Uncas.BuildPipeline.DomainServices;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.Core.Data;

namespace Uncas.BuildPipeline.Tests.Unit.Commands
{
    [TestFixture]
    public class StartEnqueuedDeploymentsHandlerTests :
        WithFixture<StartEnqueuedDeploymentsHandler>
    {
        private void SetupRepositories(
            int pipelineId,
            int environmentId)
        {
            var environment = new Environment();
            Fixture.Inject(pipelineId);
            var pipeline = Fixture.Create<Pipeline>();
            Mock<IEnvironmentRepository> environmentRepositoryMock =
                Fixture.FreezeMock<IEnvironmentRepository>();
            environmentRepositoryMock.Setup(
                er => er.GetEnvironment(environmentId)).Returns(
                    environment);
            Mock<IPipelineRepository> pipelineRepositoryMock =
                Fixture.FreezeMock<IPipelineRepository>();
            pipelineRepositoryMock.Setup(
                pr => pr.GetPipeline(pipelineId)).Returns(
                    pipeline);
            Mock<IProjectReadStore> projectReadStoreMock =
                Fixture.FreezeMock<IProjectReadStore>();
            projectReadStoreMock.Setup(x => x.GetProjectById(It.IsAny<int>())).Returns(new ProjectReadModel
                {DeploymentScript = "bla"});
        }

        private void WithDeployments(
            params Deployment[] deployments)
        {
            Mock<IDeploymentRepository> deploymentRepositoryMock =
                Fixture.FreezeMock<IDeploymentRepository>();
            deploymentRepositoryMock.Setup(
                dr => dr.GetDueDeployments(It.IsAny<PagingInfo>()))
                .Returns(new List<Deployment>(deployments));
        }

        [Test]
        public void DeployDueDeployments_NoDue_DeploysNone()
        {
            Mock<IDeploymentMethod> deploymentUtilityMock =
                Fixture.FreezeMock<IDeploymentMethod>();

            Sut.Handle(new StartEnqueuedDeployments());

            deploymentUtilityMock.Verify(
                du =>
                du.Deploy(It.IsAny<string>(), It.IsAny<Environment>(),
                          It.IsAny<ProjectReadModel>()),
                Times.Never());
        }

        [Test]
        public void DeployDueDeployments_WhenCalledSecondTime_DeploysNoneSecondTime()
        {
            Mock<IDeploymentMethodFactory> deploymentUtilityMock =
                Fixture.FreezeMock<IDeploymentMethodFactory>();
            const int pipelineId = 1;
            const int environmentId = 1;
            var deployment = new Deployment(
                pipelineId,
                environmentId);
            SetupRepositories(pipelineId, environmentId);
            WithDeployments(deployment);
            Sut.Handle(new StartEnqueuedDeployments());
            deploymentUtilityMock.Verify(
                du =>
                du.CreateDeploymentMethod(
                    It.IsAny<ProjectReadModel>(), It.IsAny<Environment>()),
                Times.Once());
            WithDeployments();

            Sut.Handle(new StartEnqueuedDeployments());

            deploymentUtilityMock.Verify(
                du =>
                du.CreateDeploymentMethod(
                    It.IsAny<ProjectReadModel>(), It.IsAny<Environment>()),
                Times.Once());
        }

        [Test]
        public void DeployDueDeployments_WhenContainsOne_DeploysOne()
        {
            var deploymentMethodMock =
                Fixture.Freeze<Mock<IDeploymentMethod>>();
            var deploymentMethodFactoryMock =
                Fixture.Freeze<Mock<IDeploymentMethodFactory>>();
            deploymentMethodFactoryMock.Setup(
                x =>
                x.CreateDeploymentMethod(It.IsAny<ProjectReadModel>(),
                                         It.IsAny<Environment>())).Returns(
                                             deploymentMethodMock.Object);
            const int pipelineId = 1;
            const int environmentId = 1;
            var deployment = new Deployment(
                pipelineId,
                environmentId);
            SetupRepositories(pipelineId, environmentId);
            WithDeployments(deployment);

            Sut.Handle(new StartEnqueuedDeployments());

            deploymentMethodFactoryMock.Verify(
                du =>
                du.CreateDeploymentMethod(
                    It.IsAny<ProjectReadModel>(), It.IsAny<Environment>()),
                Times.Once());
            deploymentMethodMock.Verify(
                x =>
                x.Deploy(It.IsAny<string>(), It.IsAny<Environment>(),
                         It.IsAny<ProjectReadModel>())
                , Times.Once());
        }
    }
}