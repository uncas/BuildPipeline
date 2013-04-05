using System;
using System.Linq.Expressions;
using Moq;
using Ploeh.AutoFixture;

namespace Uncas.BuildPipeline.Tests.Unit
{
    public static class FixtureExtensions
    {
        public static Mock<T> FreezeMock<T>(this IFixture fixture) where T : class
        {
            return fixture.Freeze<Mock<T>>();
        }

        public static TValue FreezeResult<TMock, TValue>(this IFixture fixture)
            where TMock : class
        {
            var value = fixture.Freeze<TValue>();
            return fixture.FreezeResult<TMock, TValue>(value);
        }

        public static TValue FreezeResult<TMock, TValue>(
            this IFixture fixture,
            TValue value) where TMock : class
        {
            fixture.FreezeMock<TMock>().SetReturnsDefault(value);
            return value;
        }

        public static void FreezeNullResult<TMock, TResult>(
            this IFixture fixture,
            Expression<Func<TMock, TResult>> expression)
            where TMock : class
            where TResult : class
        {
            Mock<TMock> mock =
                fixture.FreezeMock<TMock>();
            mock.Setup(expression).Returns((TResult) null);
        }
    }
}