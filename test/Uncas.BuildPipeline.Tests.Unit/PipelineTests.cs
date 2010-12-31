namespace Uncas.BuildPipeline.Tests.Unit
{
    using System;
    using System.Threading;
    using NUnit.Framework;
    using Uncas.BuildPipeline.Models;

    [TestFixture]
    public class PipelineTests
    {
        [Test]
        public void Construct()
        {
            var pipeline = GetPipeline();
        }

        [Test]
        public void IsSuccessful_PipelineWithSuccessfulStep_IsSuccessful()
        {
            var pipeline = GetPipeline();
            pipeline.AddStep(GetBuildStep(true));

            Assert.True(pipeline.IsSuccessful);
        }

        [Test]
        public void IsSuccessful_PipelineWithoutSteps_IsSuccessful()
        {
            var pipeline = GetPipeline();

            Assert.True(pipeline.IsSuccessful);
        }

        [Test]
        public void IsSuccessful_PipelineWithUnsuccessfulStep_IsNotSuccessful()
        {
            var pipeline = GetPipeline();
            pipeline.AddStep(GetBuildStep(false));

            Assert.False(pipeline.IsSuccessful);
        }

        [Test]
        public void IsSuccessful_PipelineWithMixedSuccess_IsNotSuccessful()
        {
            var pipeline = GetPipeline();
            pipeline.AddStep(GetBuildStep(false, "A"));
            pipeline.AddStep(GetBuildStep(true, "B"));

            Assert.False(pipeline.IsSuccessful);
        }

        [Test]
        public void IsSuccessful_PipelineWithMixedSuccessThatWasFixed_IsSuccessful()
        {
            var pipeline = GetPipeline();
            pipeline.AddStep(GetBuildStep(false, "A"));
            Thread.Sleep(10);
            pipeline.AddStep(GetBuildStep(true, "A"));
            pipeline.AddStep(GetBuildStep(true, "B"));

            Assert.True(pipeline.IsSuccessful);
        }

        private static Pipeline GetPipeline()
        {
            return new Pipeline(
                1,
                "My project",
                12,
                "https://svn/test/trunk",
                "https://svn/test/",
                DateTime.Now,
                "N.N.",
                @"C:\temp.zip");
        }

        private static BuildStep GetBuildStep(bool isSuccessful)
        {
            return GetBuildStep(isSuccessful, "A");
        }

        private static BuildStep GetBuildStep(
            bool isSuccessful,
            string stepName)
        {
            return new BuildStep(isSuccessful, stepName, DateTime.Now);
        }
    }
}
