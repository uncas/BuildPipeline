using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;

namespace Uncas.BuildPipeline.Tests.Integration.Repositories
{
    [TestFixture]
    public class PipelineRepositoryTests : WithBootstrapping<IPipelineRepository>
    {
        [Test]
        public void GetPipelines_PageSize1_PipelineIsIncluded()
        {
            const int pageSize = 1;
            var pipeline1 = new Pipeline(1,
                                         "myproject",
                                         "123812830",
                                         "myBranchName",
                                         DateTime.Now,
                                         "mySourceAuthor",
                                         "myPackagePath");
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