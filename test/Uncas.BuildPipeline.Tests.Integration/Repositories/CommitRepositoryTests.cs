using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;

namespace Uncas.BuildPipeline.Tests.Integration.Repositories
{
    public class CommitRepositoryTests : WithBootstrapping<CommitRepository>
    {
        [Test]
        public void Add_AddsTwice_RunsWithoutErrors()
        {
            int projectId =
                Resolve<IProjectReadStore>().AddProject(Fixture.Create<string>());
            Fixture.Inject(projectId);
            Fixture.Inject(Fixture.Create<string>());

            Sut.Add(Fixture.Create<CommitReadModel>());
            Sut.Add(Fixture.Create<CommitReadModel>());
        }

        [Test]
        public void Add_IsAdded()
        {
            int projectId =
                Resolve<IProjectReadStore>().AddProject(Fixture.Create<string>());
            Fixture.Inject(projectId);
            Sut.Add(Fixture.Create<CommitReadModel>());
        }

        [Test]
        public void GetRevisionsWithoutCommits_RunsWithoutErrors()
        {
            Sut.GetRevisionsWithoutCommits(Fixture.CreateMany<int>());
        }
    }
}