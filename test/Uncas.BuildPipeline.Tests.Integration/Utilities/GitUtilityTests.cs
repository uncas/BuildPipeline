using Moq;
using NUnit.Framework;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Tests.Integration.Utilities
{
    [TestFixture]
    public class GitUtilityTests : WithBootstrapping<GitUtility>
    {
        private const string RepoWithMerges = @"C:\Projects\OpenSource\ravendb";
        private const string FromRevision = "build-510";
        private const string ToRevision = "build-541";

        [Test]
        public void ContainsCommit()
        {
            Sut.ContainsCommit(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>());
        }

        [Test]
        public void GetBranchesMerged()
        {
            Sut.GetBranchesMerged(RepoWithMerges, FromRevision, ToRevision);
        }

        [Test]
        public void GetChangedFiles()
        {
            Sut.GetChangedFiles(RepoWithMerges, FromRevision, ToRevision);
        }

        [Test]
        public void GetLogs()
        {
            Sut.GetLogs(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<bool>());
        }

        [Test]
        public void GetShortLog()
        {
            Sut.GetShortLog(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>());
        }

        [Test]
        public void Mirror()
        {
            Sut.Mirror(
                "git://github.com/uncas/BuildPipeline.git",
                @"C:\Temp\Mirrors",
                "BuildPipeline");
        }
    }
}