using NUnit.Framework;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Tests.Integration.Utilities
{
    [TestFixture]
    public class GitUtilityTests : WithBootstrapping<GitUtility>
    {
        [Test]
        public void Mirror()
        {
            Sut.Mirror("git://github.com/uncas/BuildPipeline.git", @"C:\Temp\Mirrors", "BuildPipeline");
        }
    }
}