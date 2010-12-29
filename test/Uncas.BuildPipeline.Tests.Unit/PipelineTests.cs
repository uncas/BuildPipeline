namespace Uncas.BuildPipeline.Tests.Unit
{
    using NUnit.Framework;
    using Uncas.BuildPipeline.Web.Models;

    [TestFixture]
    public class PipelineTests
    {
        [Test]
        public void Construct()
        {
            var pipeline = new Pipeline();
        }

        [Test]
        public void IsSuccessful_PipelineWithSuccessfulStep_IsSuccessful()
        {
            var pipeline = new Pipeline();
            pipeline.AddStep(new BuildStep
            {
                IsSuccessful = true
            });

            Assert.True(pipeline.IsSuccessful);
        }

        [Test]
        public void IsSuccessful_PipelineWithoutSteps_IsSuccessful()
        {
            var pipeline = new Pipeline();

            Assert.True(pipeline.IsSuccessful);
        }

        [Test]
        public void IsSuccessful_PipelineWithUnsuccessfulStep_IsNotSuccessful()
        {
            var pipeline = new Pipeline();
            pipeline.AddStep(new BuildStep
            {
                IsSuccessful = false
            });

            Assert.False(pipeline.IsSuccessful);
        }

        [Test]
        public void IsSuccessful_PipelineWithMixedSuccess_IsNotSuccessful()
        {
            var pipeline = new Pipeline();
            pipeline.AddStep(new BuildStep
            {
                IsSuccessful = false,
                StepName = "A"
            });
            pipeline.AddStep(new BuildStep
            {
                IsSuccessful = true,
                StepName = "B"
            });

            Assert.False(pipeline.IsSuccessful);
        }
    }
}
