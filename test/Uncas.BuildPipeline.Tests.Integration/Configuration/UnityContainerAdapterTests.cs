using NUnit.Framework;
using Uncas.BuildPipeline.Tests.Unit;
using Uncas.BuildPipeline.Web.Configuration;

namespace Uncas.BuildPipeline.Tests.Integration.Configuration
{
    public class UnityContainerAdapterTests : WithFixture<UnityContainerAdapter>
    {
        [Test]
        public void Resolve_ILogger_Logger()
        {
            var logger = Sut.Resolve<ILogger>();

            Assert.IsInstanceOf<Logger>(logger);
        }

        [Test]
        public void TryResolve_ILogger_Logger()
        {
            var logger = Sut.TryResolve<ILogger>();

            Assert.IsInstanceOf<Logger>(logger);
        }
    }
}