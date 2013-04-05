using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.Core.Data;

namespace Uncas.BuildPipeline.Tests.Integration.Repositories
{
    public class EnvironmentRepositoryTests : WithBootstrapping<EnvironmentRepository>
    {
        [Test]
        public void GetEnvironment_Existing_GetsExisting()
        {
            IEnumerable<Environment> environments =
                Sut.GetEnvironments(Fixture.Create<PagingInfo>());
            Assume.That(environments.Count(), Is.GreaterThan(0));

            Environment environment = Sut.GetEnvironment(environments.First().Id);

            Assert.NotNull(environment);
        }

        [Test]
        public void GetEnvironments_GetsSome()
        {
            IEnumerable<Environment> environments =
                Sut.GetEnvironments(Fixture.Create<PagingInfo>());

            Assert.That(environments.Count(), Is.GreaterThan(0));
        }
    }
}