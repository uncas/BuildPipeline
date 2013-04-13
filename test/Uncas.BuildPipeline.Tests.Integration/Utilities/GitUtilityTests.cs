using System.Collections.Generic;
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
            IEnumerable<string> branchesMerged =
                Sut.GetBranchesMerged(RepoWithMerges, FromRevision, ToRevision);

            CollectionAssert.IsNotEmpty(branchesMerged);
        }

        [Test]
        public void GetBranchesMerged_ExcludeMaster()
        {
            IEnumerable<string> branchesMerged =
                Sut.GetBranchesMerged(RepoWithMerges, FromRevision, ToRevision, "master");

            CollectionAssert.IsNotEmpty(branchesMerged);
        }

        [Test]
        public void GetChangedFiles()
        {
            IEnumerable<string> changedFiles =
                Sut.GetChangedFiles(RepoWithMerges, FromRevision, ToRevision);

            CollectionAssert.IsNotEmpty(changedFiles);
        }

        [Test]
        public void GetLogs()
        {
            Sut.GetLogs(
                RepoWithMerges,
                FromRevision,
                ToRevision,
                It.IsAny<string>(),
                1,
                It.IsAny<bool>());
        }

        [Test]
        public void GetLogs_NonExistingSha()
        {
            Sut.GetLogs(
                @"C:\Temp\Mirrors\BuildPipeline",
                "abc",
                "def",
                It.IsAny<string>(),
                1,
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