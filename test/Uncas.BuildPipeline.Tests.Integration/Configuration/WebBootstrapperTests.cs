using NUnit.Framework;
using Uncas.BuildPipeline.Tests.Unit;
using Uncas.BuildPipeline.Web.Configuration;

namespace Uncas.BuildPipeline.Tests.Integration.Configuration
{
    public class WebBootstrapperTests : WithFixture
    {
        private interface IHaveNoImplementation
        {
        }

        [Test]
        public void InitialiseForWeb_RunsWithoutErrors()
        {
            WebBootstrapper.InitialiseForWeb();
        }

        [Test]
        public void Resolve_ILogger_ResolvesLogger()
        {
            var logger = WebBootstrapper.Resolve<ILogger>();

            Assert.IsInstanceOf<Logger>(logger);
        }

        [Test]
        public void TryResolve_IHaveNoImplementation_None()
        {
            var noImplementation = WebBootstrapper.TryResolve<IHaveNoImplementation>();

            Assert.Null(noImplementation);
        }

        [Test]
        public void TryResolve_ILogger_ResolvesLogger()
        {
            var logger = WebBootstrapper.TryResolve<ILogger>();

            Assert.IsInstanceOf<Logger>(logger);
        }
    }
}