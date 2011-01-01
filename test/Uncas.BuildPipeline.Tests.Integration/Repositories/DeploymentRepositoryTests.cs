namespace Uncas.BuildPipeline.Tests.Integration.Repositories
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Uncas.BuildPipeline.Models;
    using Uncas.BuildPipeline.Repositories;

    [TestFixture]
    public class DeploymentRepositoryTests
    {
        private IDeploymentRepository deploymentRepository;

        [SetUp]
        public void BeforeEach()
        {
            this.deploymentRepository = new DeploymentRepository();
        }

        [Test]
        [Ignore]
        public void AddDeployment_WhenAdded_CanBeRetrieved()
        {
            const int pipelineId=1;
            const int environmentId=1;
            DateTime scheduledStart = DateTime.Now.AddMinutes(3d);
            var deployment = new Deployment(
                pipelineId,
                environmentId,
                scheduledStart);
            this.deploymentRepository.AddDeployment(deployment);

            var result = 
                this.deploymentRepository.GetDeployments(pipelineId);

            Assert.AreEqual(1, result.Count());
        }

        // TODO: Test that when a deployment has been deployed, it should not be deployed again....
    }
}