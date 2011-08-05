namespace Uncas.BuildPipeline.Tests.Integration.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Uncas.BuildPipeline.Models;
    using Uncas.BuildPipeline.Repositories;

    [TestFixture]
    public class PipelineRepositoryTests
    {
        private readonly IPipelineRepository _pipelineRepository =
            new PipelineRepository(new BuildPipelineRepositoryConfiguration());

        [Test]
        public void GetPipelines_PageSize1_StepsAreIncluded()
        {
            const int pageSize = 1;
            _pipelineRepository.AddPipeline();

            Pipeline pipeline =
                _pipelineRepository.GetPipelines(pageSize).FirstOrDefault();

            Assert.NotNull(pipeline);
            Assert.NotNull(pipeline.ProjectName);
            Assert.Greater(pipeline.SourceRevision, 0);
            Assert.True(pipeline.Steps.Count() > 0);
        }

        [Test]
        public void GetPipelines_PageSize2_GetsMax2()
        {
            const int pageSize = 2;

            IEnumerable<Pipeline> pipelines =
                _pipelineRepository.GetPipelines(pageSize);

            Assert.True(pipelines.Count() <= pageSize);
        }
    }
}