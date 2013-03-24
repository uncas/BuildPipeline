﻿using System;
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
            new PipelineRepository();

        private TransactionScope _scope;

        [Test]
        public void GetPipelines_PageSize1_PipelineIsIncluded()
        {
            const int pageSize = 1;
            var pipeline1 = new Pipeline(1,
                                         "myproject",
                                         123812830,
                                         "mySourceUrl",
                                         "mySourceUrlBase",
                                         DateTime.Now,
                                         "mySourceAuthor",
                                         "myPackagePath");
            _pipelineRepository.AddPipeline(pipeline1);

            Pipeline pipeline =
                _pipelineRepository.GetPipelines(pageSize).FirstOrDefault();

            Assert.NotNull(pipeline);
            Assert.AreEqual(pipeline1.ProjectName, pipeline.ProjectName);
            Assert.AreEqual(pipeline1.SourceRevision, pipeline.SourceRevision);
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