namespace Uncas.BuildPipeline.Tests.Integration
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Uncas.BuildPipeline.Web.Models;

    [TestFixture]
    public class PipelineRepositoryTests
    {
        private readonly PipelineRepository pipelineRepository = 
            new PipelineRepository();

        [Test]
        public void GetPipelines_PageSize2_GetsMax2()
        {
            const int pageSize = 2;

            IEnumerable<Pipeline> pipelines =
                this.pipelineRepository.GetPipelines(pageSize);

            Assert.True(pipelines.Count() <= pageSize);
        }

        [Test]
        public void GetPipelines_PageSize1_StepsAreIncluded()
        {
            const int pageSize = 1;

            Pipeline pipeline =
                this.pipelineRepository.GetPipelines(pageSize).FirstOrDefault();

            Assert.NotNull(pipeline);
            Assert.NotNull(pipeline.ProjectName);
            Assert.Greater(pipeline.SourceRevision, 0);
            Assert.True(pipeline.Steps.Count() > 0);
        }
    }
}