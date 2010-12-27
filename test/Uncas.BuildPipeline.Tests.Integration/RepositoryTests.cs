namespace Uncas.BuildPipeline.Tests.Integration
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Uncas.BuildPipeline.Web.Models;

    [TestFixture]
    public class RepositoryTests
    {
        private readonly Repository repository = new Repository();

        [Test]
        public void GetBuilds_PageSize2_GetsMax2()
        {
            const int pageSize = 2;

            IEnumerable<Build> builds =
                this.repository.GetBuilds(pageSize);

            Assert.True(builds.Count() <= pageSize);
        }

        [Test]
        public void GetBuilds_PageSize1_StepsAreIncluded()
        {
            const int pageSize = 1;

            Build build =
                this.repository.GetBuilds(pageSize).FirstOrDefault();

            Assert.NotNull(build);
            Assert.NotNull(build.ProjectName);
            Assert.Greater(build.SourceRevision, 0);
            Assert.True(build.Steps.Count() > 0);
        }
    }
}