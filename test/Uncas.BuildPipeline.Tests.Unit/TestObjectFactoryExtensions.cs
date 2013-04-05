using System;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.Tests.Unit
{
    public static class TestObjectFactoryExtensions
    {
        public static Pipeline CreatePipeline(this IFixture fixture)
        {
            return new Pipeline(fixture.Create<int>(),
                                fixture.Create<string>(),
                                fixture.Create<string>(),
                                fixture.Create<string>(),
                                fixture.Create<DateTime>(),
                                fixture.Create<string>());
        }
    }
}