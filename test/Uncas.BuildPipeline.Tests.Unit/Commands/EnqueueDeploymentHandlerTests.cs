using System;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Commands;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Environment = Uncas.BuildPipeline.Models.Environment;

namespace Uncas.BuildPipeline.Tests.Unit.Commands
{
    public class EnqueueDeploymentHandlerTests : WithFixture<EnqueueDeploymentHandler>
    {
        private void SetupRepositories(
            int pipelineId,
            int environmentId)
        {
            var environment = new Environment();
            Fixture.Inject(pipelineId);
            var pipeline = Fixture.Create<Pipeline>();
            Mock<IEnvironmentRepository> environmentRepositoryMock = Fixture.FreezeMock<IEnvironmentRepository>();
            environmentRepositoryMock.Setup(
                er => er.GetEnvironment(environmentId)).Returns(
                    environment);
            Mock<IPipelineRepository> pipelineRepositoryMock = Fixture.FreezeMock<IPipelineRepository>();
            pipelineRepositoryMock.Setup(
                pr => pr.GetPipeline(pipelineId)).Returns(
                    pipeline);
        }

        [Test]
        public void ScheduleDeployment_NonExistingInput_IsNotScheduled()
        {
            const int pipelineId = 1;
            const int environmentId = 1;
            Mock<IDeploymentRepository> deploymentRepositoryMock = Fixture.FreezeMock<IDeploymentRepository>();
            Mock<IPipelineRepository> pipelineRepositoryMock = Fixture.FreezeMock<IPipelineRepository>();
            pipelineRepositoryMock.Setup(
                pr => pr.GetPipeline(pipelineId)).Returns(
                    (Pipeline) null);

            Assert.Throws<ArgumentException>(() => Sut.Handle(new EnqueueDeployment(
                                                                  pipelineId,
                                                                  environmentId)));

            deploymentRepositoryMock.Verify(
                dr => dr.AddDeployment(It.IsAny<Deployment>()),
                Times.Never());
        }

        [Test]
        public void ScheduleDeployment_ValidCommand_IsScheduled()
        {
            const int pipelineId = 1;
            const int environmentId = 1;
            SetupRepositories(pipelineId, environmentId);
            Mock<IDeploymentRepository> deploymentRepositoryMock = Fixture.FreezeMock<IDeploymentRepository>();

            Sut.Handle(new EnqueueDeployment(
                           pipelineId,
                           environmentId));

            deploymentRepositoryMock.Verify(
                dr => dr.AddDeployment(It.IsAny<Deployment>()),
                Times.Once());
        }
    }
}