using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Commands;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Tests.Unit.Commands
{
    public class UpdateGitMirrorsHandlerTests : WithFixture<UpdateGitMirrorsHandler>
    {
        [Test]
        public void Handle()
        {
            Fixture.FreezeResult<IProjectReadStore, IEnumerable<ProjectReadModel>>(
                Fixture.CreateMany<ProjectReadModel>(2));
            Mock<IGitUtility> gitUtilityMock = Fixture.FreezeMock<IGitUtility>();

            Sut.Handle(Fixture.Create<UpdateGitMirrors>());

            gitUtilityMock.Verify(x => x.Mirror(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                                  Times.Exactly(2));
        }
    }
}