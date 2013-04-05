using NUnit.Framework;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Tests.Unit.Utilities
{
    [TestFixture]
    public class GitHubLinkTests
    {
        [Test]
        public void Branch()
        {
            GitHubLink.Branch("bla", "master");
        }

        [Test]
        public void Branch_EmptyRepo()
        {
            GitHubLink.Branch(null, "master");
        }

        [Test]
        public void Compare()
        {
            GitHubLink.Compare("bla", "master");
        }

        [Test]
        public void Compare_EmptyRepo()
        {
            GitHubLink.Compare(null, "master");
        }

        [Test]
        public void Compare_Relative()
        {
            GitHubLink.Compare("bla", "master", "develop");
        }

        [Test]
        public void Compare_Relative_EmptyRepo()
        {
            GitHubLink.Compare(null, "master", "develop");
        }

        [Test]
        public void Revision()
        {
            GitHubLink.Revision("bla", "master");
        }

        [Test]
        public void Revision_EmptyRepo()
        {
            GitHubLink.Revision(null, "master");
        }
    }
}