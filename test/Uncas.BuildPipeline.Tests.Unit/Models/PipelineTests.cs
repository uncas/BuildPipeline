using System;
using System.Threading;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.Tests.Unit.Models
{
    [TestFixture]
    public class PipelineTests : WithFixture
    {
        private Pipeline GetPipeline()
        {
            return Fixture.Create<Pipeline>();
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

        [Test]
        public void Construct()
        {
            Pipeline pipeline = GetPipeline();
        }

        [Test]
        public void IsSuccessful_PipelineWithMixedSuccessThatWasFixed_IsSuccessful()
        {
            Pipeline pipeline = GetPipeline();
            pipeline.AddStep(GetBuildStep(false, "A"));
            Thread.Sleep(10);
            pipeline.AddStep(GetBuildStep(true, "A"));
            pipeline.AddStep(GetBuildStep(true, "B"));

            Assert.True(pipeline.IsSuccessful);
        }

        [Test]
        public void IsSuccessful_PipelineWithMixedSuccess_IsNotSuccessful()
        {
            Pipeline pipeline = GetPipeline();
            pipeline.AddStep(GetBuildStep(false, "A"));
            pipeline.AddStep(GetBuildStep(true, "B"));

            Assert.False(pipeline.IsSuccessful);
        }

        [Test]
        public void IsSuccessful_PipelineWithSuccessfulStep_IsSuccessful()
        {
            Pipeline pipeline = GetPipeline();
            pipeline.AddStep(GetBuildStep(true));

            Assert.True(pipeline.IsSuccessful);
        }

        [Test]
        public void IsSuccessful_PipelineWithUnsuccessfulStep_IsNotSuccessful()
        {
            Pipeline pipeline = GetPipeline();
            pipeline.AddStep(GetBuildStep(false));

            Assert.False(pipeline.IsSuccessful);
        }

        [Test]
        public void IsSuccessful_PipelineWithoutSteps_IsSuccessful()
        {
            Pipeline pipeline = GetPipeline();

            Assert.True(pipeline.IsSuccessful);
        }
    }
}