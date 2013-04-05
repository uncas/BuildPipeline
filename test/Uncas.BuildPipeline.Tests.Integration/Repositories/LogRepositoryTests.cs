using System;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Repositories;

namespace Uncas.BuildPipeline.Tests.Integration.Repositories
{
    public class LogRepositoryTests : WithBootstrapping<LogRepository>
    {
        [Test]
        public void Add_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Sut.Add(null));
        }

        [Test]
        public void Add_Ok()
        {
            Sut.Add(Fixture.Create<LogData>());
        }
    }
}