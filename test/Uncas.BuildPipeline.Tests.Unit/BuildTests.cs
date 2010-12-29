namespace Uncas.BuildPipeline.Tests.Unit
{
    using NUnit.Framework;
    using Uncas.BuildPipeline.Web.Models;

    [TestFixture]
    public class BuildTests
    {
        [Test]
        public void Construct()
        {
            var build = new Build();
        }
    }
}
