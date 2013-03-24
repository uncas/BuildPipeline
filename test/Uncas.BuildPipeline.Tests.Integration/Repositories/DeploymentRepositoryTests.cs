using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;

namespace Uncas.BuildPipeline.Tests.Integration.Repositories
{
    [TestFixture]
    public class DeploymentRepositoryTests : WithBootstrapping<IDeploymentRepository>
    {
        private int AddPipeline()
        {
            var pipeline = new Pipeline(1,
                                        "Bla",
                                        1,
                                        "bla",
                                        "bla",
                                        DateTime.Now,
                                        "bla",
                                        "bla");
            Resolve<IPipelineRepository>().AddPipeline(pipeline);
            return pipeline.Id;
        }

        [Test]
        public void AddDeployment_WhenAdded_CanBeRetrieved()
        {
            const int environmentId = 1;
            int pipelineId = AddPipeline();
            var deployment = new Deployment(pipelineId, environmentId);

            Sut.AddDeployment(deployment);

            IEnumerable<Deployment> result = Sut.GetDeployments(pipelineId);
            Assert.True(result.Any());
        }

        [Test]
        public void GetByEnvironment_X()
        {
            Sut.GetByEnvironment(1);
        }

        [Test]
        public void UpdateDeployment_WhenStarted_StartedIsUpdated()
        {
            int pipelineId = AddPipeline();
            var added = new Deployment(pipelineId, 1);
            Sut.AddDeployment(added);
            Thread.Sleep(10);
            added.Start();
            int deploymentId = added.Id.Value;

            Sut.UpdateDeployment(added);

            Deployment updated = Sut.GetDeployment(deploymentId);
            Assert.NotNull(updated.Started);
            Assert.True(updated.Started > added.Created);
        }
    }
}