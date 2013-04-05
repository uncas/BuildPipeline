using NUnit.Framework;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Tests.Unit.Utilities
{
    public class GitLogTests : WithFixture<GitLog>
    {
        [Test]
        public void ToString_Ok()
        {
            string s = Sut.ToString();

            Assert.That(s, Is.Not.Null);
        }
    }
}