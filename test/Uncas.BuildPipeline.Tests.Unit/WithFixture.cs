using System;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Uncas.BuildPipeline.Tests.Unit
{
    public abstract class WithFixture<T> : WithFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetupSut()
        {
            _sut = new Lazy<T>(Fixture.Create<T>);
        }

        #endregion

        private Lazy<T> _sut;

        public T Sut
        {
            get { return _sut.Value; }
        }
    }

    [TestFixture]
    public abstract class WithFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetupFixture()
        {
            _fixture = new Lazy<IFixture>(TestObjectFactory.SetupFixture);
        }

        #endregion

        private Lazy<IFixture> _fixture;

        public IFixture Fixture
        {
            get { return _fixture.Value; }
        }
    }
}