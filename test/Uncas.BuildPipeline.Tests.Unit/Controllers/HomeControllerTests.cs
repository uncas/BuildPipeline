using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Commands;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Utilities;
using Uncas.BuildPipeline.Web.Controllers;

namespace Uncas.BuildPipeline.Tests.Unit.Controllers
{
    public class HomeControllerTests : WithFixture<HomeController>
    {
        [Test]
        public void About()
        {
            ActionResult actionResult = Sut.About();

            Assert.IsInstanceOf<ViewResult>(actionResult);
        }

        [Test]
        public void DeployPost()
        {
            Mock<ICommandBus> mock = Fixture.FreezeMock<ICommandBus>();

            ActionResult actionResult = Sut.Deploy(Fixture.Create<int>(),
                                                   Fixture.Create<int>());

            mock.Verify(x => x.Publish(It.IsAny<ICommand>()), Times.Once());
            Assert.IsInstanceOf<RedirectToRouteResult>(actionResult);
        }

        [Test]
        public void Deploy_ExistingPipeline()
        {
            Fixture.FreezeResult<IPipelineRepository, Pipeline>();

            ActionResult actionResult = Sut.Deploy(Fixture.Create<int>());

            Assert.IsInstanceOf<ViewResult>(actionResult);
        }

        [Test]
        public void Deploy_NonExistingPipeline_NotFoundResult()
        {
            Fixture.FreezeNullResult<IPipelineRepository, Pipeline>(
                x => x.GetPipeline(It.IsAny<int>()));

            ActionResult actionResult = Sut.Deploy(Fixture.Create<int>());

            Assert.IsInstanceOf<HttpNotFoundResult>(actionResult);
        }

        [Test]
        public void Download_ExistingFile_Found()
        {
            Fixture.FreezeResult<IFileUtility, bool>(true);

            ActionResult actionResult = Sut.Download(Fixture.Create<string>());

            Assert.IsInstanceOf<FilePathResult>(actionResult);
        }

        [Test]
        public void Download_NonExistingFile_NotFound()
        {
            ActionResult actionResult = Sut.Download(Fixture.Create<string>());

            Assert.IsInstanceOf<HttpNotFoundResult>(actionResult);
        }

        [Test]
        public void Index()
        {
            ActionResult actionResult = Sut.Index();

            Assert.IsInstanceOf<ViewResult>(actionResult);
        }
    }
}