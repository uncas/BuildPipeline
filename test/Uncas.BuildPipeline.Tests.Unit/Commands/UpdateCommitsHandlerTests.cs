using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Commands;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Tests.Unit.Commands
{
    public class UpdateCommitsHandlerTests : WithFixture<UpdateCommitsHandler>
    {
        private void WhenProjectContainsCommits(int numberOfCommits = 3)
        {
            IEnumerable<ProjectReadModel> projects =
                Fixture.FreezeResult<IProjectReadStore, IEnumerable<ProjectReadModel>>();
            Fixture.Inject(projects.First().ProjectId);
            Fixture.FreezeResult<ICommitRepository, IEnumerable<CommitReadModel>>(
                Fixture.CreateMany<CommitReadModel>(numberOfCommits));
        }

        [Test]
        public void GivenProjectsContainsOneCommit_WhenALogIsRetrieved_ThenCommitIsSaved()
        {
            WhenProjectContainsCommits(1);
            Fixture.FreezeResult<IGitUtility, IEnumerable<GitLog>>(
                Fixture.CreateMany<GitLog>(1));
            Mock<ICommitRepository> mock = Fixture.FreezeMock<ICommitRepository>();

            Sut.Handle(Fixture.Create<UpdateCommits>());

            mock.Verify(x => x.Add(It.IsAny<CommitReadModel>()), Times.Once());
        }

        [Test]
        public void GivenProjectsExist_WhenProjectsContainCommits_LogsAreRequested()
        {
            WhenProjectContainsCommits();
            Mock<IGitUtility> mock = Fixture.FreezeMock<IGitUtility>();

            Sut.Handle(Fixture.Create<UpdateCommits>());

            mock.Verify(
                x =>
                x.GetLogs(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                          It.IsAny<string>(),
                          It.IsAny<int>(), It.IsAny<bool>()), Times.AtLeastOnce());
        }
    }
}