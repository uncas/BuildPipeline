using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;

namespace Uncas.BuildPipeline.Tests.Integration.Repositories
{
    [TestFixture]
    public class PipelineRepositoryTests
    {
        #region Setup/Teardown

        [SetUp]
        public void BeforeEach()
        {
            _scope = new TransactionScope();
        }

        [TearDown]
        public void AfterEach()
        {
            _scope.Dispose();
        }

        #endregion

        private readonly IPipelineRepository _pipelineRepository =
            new PipelineRepository(new BuildPipelineRepositoryConfiguration());

        private TransactionScope _scope;

        [Test]
        public void GetPipelines_PageSize1_PipelineIsIncluded()
        {
            const int pageSize = 1;
            _pipelineRepository.AddPipeline(new Pipeline(1,
                                                         "myproject",
                                                         123812830,
                                                         "mySourceUrl",
                                                         "mySourceUrlBase",
                                                         DateTime.Now,
                                                         "mySourceAuthor",
                                                         "myPackagePath"));

            Pipeline pipeline =
                _pipelineRepository.GetPipelines(pageSize).FirstOrDefault();

            Assert.NotNull(pipeline);
            Assert.NotNull(pipeline.ProjectName);
            Assert.Greater(pipeline.SourceRevision, 0);
        }

        [Test]
        public void GetPipelines_PageSize2_GetsMax2()
        {
            const int pageSize = 2;

            IEnumerable<Pipeline> pipelines = _pipelineRepository.GetPipelines(pageSize);

            Assert.True(pipelines.Count() <= pageSize);
        }
    }
}