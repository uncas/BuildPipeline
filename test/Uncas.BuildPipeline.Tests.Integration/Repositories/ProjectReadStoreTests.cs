using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;

namespace Uncas.BuildPipeline.Tests.Integration.Repositories
{
    public class ProjectReadStoreTests : WithBootstrapping<ProjectReadStore>
    {
        [Test]
        public void GetProjectById_ExistingId_GetsExisting()
        {
            int projectId = Sut.AddProject(Fixture.Create<string>());

            ProjectReadModel project = Sut.GetProjectById(projectId);

            Assert.NotNull(project);
        }

        [Test]
        public void GetProjectById_NonExistingId_GetsNone()
        {
            ProjectReadModel project = Sut.GetProjectById(-1);

            Assert.Null(project);
        }

        [Test]
        public void GetProjects()
        {
            Sut.GetProjects();
        }

        [Test]
        public void Update()
        {
            Sut.Update(Fixture.Create<ProjectReadModel>());
        }
    }
}