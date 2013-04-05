using System;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Uncas.BuildPipeline.Tests.Unit
{
    public class LoggerTests : WithFixture<Logger>
    {
        [Test]
        public void Debug()
        {
            Sut.Debug(Fixture.Create<string>());
        }

        [Test]
        public void Error()
        {
            Sut.Error(Fixture.Create<string>());
        }

        [Test]
        public void Error_Exception()
        {
            Sut.Error(Fixture.Create<Exception>(), Fixture.Create<string>());
        }

        [Test]
        public void Info()
        {
            Sut.Info(Fixture.Create<string>());
        }
    }
}