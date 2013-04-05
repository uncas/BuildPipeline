using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Web.Controllers;

namespace Uncas.BuildPipeline.Tests.Unit.Controllers
{
    public class CustomApiControllerTests : WithFixture<CustomApiController>
    {
        [Test]
        public void Packages_File_Valid()
        {
            Sut.Packages(Fixture.Create<string>());
        }
    }
}