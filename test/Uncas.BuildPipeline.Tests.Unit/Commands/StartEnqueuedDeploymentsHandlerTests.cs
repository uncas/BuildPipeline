using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Commands;
using Uncas.BuildPipeline.DomainServices;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.Core.Data;
using Environment = Uncas.BuildPipeline.Models.Environment;

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

        private void Handle()
        {
            Sut.Handle(Fixture.Create<StartEnqueuedDeployments>());
        }

        [Test]
        public void DeployDueDeployments_NoDue_DeploysNone()
        {
            Mock<IDeploymentMethod> deploymentUtilityMock =
                Fixture.FreezeMock<IDeploymentMethod>();

            Handle();

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
            Handle();
            deploymentUtilityMock.Verify(
                du =>
                du.CreateDeploymentMethod(
                    It.IsAny<ProjectReadModel>(), It.IsAny<Environment>()),
                Times.Once());
            WithDeployments();

            Handle();

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

            Handle();

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

        [Test]
        public void Handle_NonExistingEnvironment_DoesNotDeploy()
        {
            Mock<IDeploymentMethodFactory> deploymentMethodFactoryMock =
                Fixture.FreezeMock<IDeploymentMethodFactory>();
            Mock<IEnvironmentRepository> repositoryMock =
                Fixture.FreezeMock<IEnvironmentRepository>();
            Fixture.FreezeResult<IDeploymentRepository, IEnumerable<Deployment>>(
                Fixture.CreateMany<Deployment>());
            Fixture.FreezeResult<IPipelineRepository, Pipeline>();
            Fixture.FreezeNullResult<IEnvironmentRepository, Environment>(
                x => x.GetEnvironment(It.IsAny<int>()));

            Assert.Throws<ArgumentException>(Handle);

            repositoryMock.Verify(x => x.GetEnvironment(It.IsAny<int>()),
                                  Times.Once());
            deploymentMethodFactoryMock.Verify(
                x =>
                x.CreateDeploymentMethod(It.IsAny<ProjectReadModel>(),
                                         It.IsAny<Environment>()), Times.Never());
        }

        [Test]
        public void Handle_NonExistingPipeline_DoesNotDeploy()
        {
            Mock<IDeploymentMethodFactory> deploymentMethodFactoryMock =
                Fixture.FreezeMock<IDeploymentMethodFactory>();
            Mock<IPipelineRepository> repositoryMock =
                Fixture.FreezeMock<IPipelineRepository>();
            Fixture.FreezeResult<IDeploymentRepository, IEnumerable<Deployment>>(
                Fixture.CreateMany<Deployment>());
            Fixture.FreezeNullResult<IPipelineRepository, Pipeline>(
                x => x.GetPipeline(It.IsAny<int>()));

            Assert.Throws<ArgumentException>(Handle);

            repositoryMock.Verify(x => x.GetPipeline(It.IsAny<int>()),
                                  Times.Once());
            deploymentMethodFactoryMock.Verify(
                x =>
                x.CreateDeploymentMethod(It.IsAny<ProjectReadModel>(),
                                         It.IsAny<Environment>()), Times.Never());
        }

        [Test]
        public void Handle_NonExistingProject_DoesNotDeploy()
        {
            Mock<IDeploymentMethodFactory> deploymentMethodFactoryMock =
                Fixture.FreezeMock<IDeploymentMethodFactory>();
            Mock<IProjectReadStore> repositoryMock =
                Fixture.FreezeMock<IProjectReadStore>();
            Fixture.FreezeResult<IDeploymentRepository, IEnumerable<Deployment>>(
                Fixture.CreateMany<Deployment>());
            Fixture.FreezeResult<IPipelineRepository, Pipeline>();
            Fixture.FreezeResult<IEnvironmentRepository, Environment>();
            Fixture.FreezeNullResult<IProjectReadStore, ProjectReadModel>(
                x => x.GetProjectById(It.IsAny<int>()));

            Assert.Throws<InvalidOperationException>(Handle);

            repositoryMock.Verify(x => x.GetProjectById(It.IsAny<int>()),
                                  Times.Once());
            deploymentMethodFactoryMock.Verify(
                x =>
                x.CreateDeploymentMethod(It.IsAny<ProjectReadModel>(),
                                         It.IsAny<Environment>()), Times.Never());
        }
    }
}