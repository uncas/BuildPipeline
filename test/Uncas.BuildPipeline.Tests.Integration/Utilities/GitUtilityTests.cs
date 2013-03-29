using NUnit.Framework;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Tests.Integration.Utilities
{
    [TestFixture]
    public class GitUtilityTests
    {
        [Test]
        public void Mirror()
        {
            var gitUtility = new GitUtility();
            gitUtility.Mirror("git://github.com/uncas/BuildPipeline.git", @"C:\Temp\Mirrors", "BuildPipeline");
        }
    }
}