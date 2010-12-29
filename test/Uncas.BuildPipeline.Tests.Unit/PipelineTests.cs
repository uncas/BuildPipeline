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
    }
}
