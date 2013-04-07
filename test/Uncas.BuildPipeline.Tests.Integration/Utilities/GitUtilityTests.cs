using System.IO;
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

        [TestCase("BuildPipeline", "git://github.com/uncas/BuildPipeline.git", true)]
        [TestCase("BuildPipelineLocal", "C:/Projects/OpenSource/BuildPipeline", true)]
        [TestCase("NonExisting", "C:/Projects/OpenSource/NonExisting", false)]
        public void Mirror(string mirrorName, string remoteUrl, bool mirrorCreated)
        {
            const string testMirrorsRoot = @"C:\Temp\TestMirrors";

            Sut.Mirror(remoteUrl, testMirrorsRoot, mirrorName);

            string mirrorPath = Path.Combine(testMirrorsRoot, mirrorName);
            Assert.That(Directory.Exists(mirrorPath), Is.EqualTo(mirrorCreated));
        }

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
    }
}