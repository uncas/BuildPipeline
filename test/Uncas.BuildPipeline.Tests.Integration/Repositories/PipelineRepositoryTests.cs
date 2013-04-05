using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;

namespace Uncas.BuildPipeline.Tests.Integration.Repositories
{
    [TestFixture]
    public class PipelineRepositoryTests : WithBootstrapping<IPipelineRepository>
    {
        [Test]
        public void GetPipeline_ExistingId_GetsExisting()
        {
            var original = Fixture.Create<Pipeline>();
            Sut.AddPipeline(original);

            Pipeline retrieved = Sut.GetPipeline(original.PipelineId);

            Assert.NotNull(retrieved);
        }

        [Test]
        public void GetPipeline_NonExistingId_GetsNull()
        {
            Pipeline pipeline = Sut.GetPipeline(-1);
            Assert.Null(pipeline);
        }

        [Test]
        public void GetPipelines_PageSize1_PipelineIsIncluded()
        {
            const int pageSize = 1;
            var pipeline1 = Fixture.Create<Pipeline>();
            pipeline1.AddStep(Fixture.Create<BuildStep>());
            Sut.AddPipeline(pipeline1);

            Pipeline pipeline = Sut.GetPipelines(pageSize).FirstOrDefault();

            Assert.NotNull(pipeline);
            Assert.AreEqual(pipeline1.ProjectName, pipeline.ProjectName);
            Assert.AreEqual(pipeline1.Revision, pipeline.Revision);
        }

        [Test]
        public void GetPipelines_PageSize2_GetsMax2()
        {
            const int pageSize = 2;

            IEnumerable<Pipeline> pipelines = Sut.GetPipelines(pageSize);

            Assert.True(pipelines.Count() <= pageSize);
        }
    }
}