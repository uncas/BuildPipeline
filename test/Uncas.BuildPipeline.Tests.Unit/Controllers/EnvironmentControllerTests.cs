using System.Collections.Generic;
using System.Web.Mvc;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Web.Controllers;

namespace Uncas.BuildPipeline.Tests.Unit.Controllers
{
    public class EnvironmentControllerTests : WithFixture<EnvironmentController>
    {
        [Test]
        public void Index()
        {
            Fixture.FreezeResult<IEnvironmentRepository, IEnumerable<Environment>>(
                Fixture.CreateMany<Environment>());

            ActionResult actionResult = Sut.Index();

            Assert.IsInstanceOf<ViewResult>(actionResult);
        }
    }
}