using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;

namespace Uncas.BuildPipeline.Tests.Integration.Repositories
{
    [TestFixture]
    public class DeploymentRepositoryTests
    {
        #region Setup/Teardown

        [SetUp]
        public void BeforeEach()
        {
            // TODO: When a dedicated test database is in place, use the real repository here:
            _deploymentRepository = new FakeDeploymentRepository();
        }

        #endregion

        private IDeploymentRepository _deploymentRepository;

        [Test]
        public void AddDeployment_WhenAdded_CanBeRetrieved()
        {
            const int pipelineId = 1;
            const int environmentId = 1;
            var deployment = new Deployment(pipelineId, environmentId);
            _deploymentRepository.AddDeployment(deployment);

            IEnumerable<Deployment> result =
                _deploymentRepository.GetDeployments(pipelineId);

            Assert.True(result.Any());
        }

        [Test]
        public void UpdateDeployment_WhenStarted_StartedIsUpdated()
        {
            // Arrange:
            var added = new Deployment(1, 1);
            _deploymentRepository.AddDeployment(added);
            Thread.Sleep(10);
            added.Start();
            int deploymentId = added.Id.Value;

            // Act:
            _deploymentRepository.UpdateDeployment(added);

            // Assert:
            Deployment updated = _deploymentRepository.GetDeployment(deploymentId);
            Assert.NotNull(updated.Started);
            Assert.True(updated.Started > added.Created);
        }
    }
}