namespace Uncas.BuildPipeline.Tests.Integration
{
    using System.Linq;
    using NUnit.Framework;
    using Uncas.BuildPipeline.Web.Models;

    [TestFixture]
    public class RepositoryTests
    {
        private readonly Repository repository = new Repository();

        [Test]
        public void GetBuilds_PageSize10_GetsMax10()
        {
            const int pageSize = 10;
            
            var builds = this.repository.GetBuilds(pageSize);

            Assert.True(builds.Count() <= pageSize);
        }
    }
}
