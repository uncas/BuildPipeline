using System.Transactions;
using NUnit.Framework;
using Uncas.BuildPipeline.Tests.Unit;

namespace Uncas.BuildPipeline.Tests.Integration
{
    public abstract class WithBootstrapping<T> : WithFixture where T : class
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUpBootstrapping()
        {
            _scope = new TransactionScope();
            _sut = null;
        }

        [TearDown]
        public void TearDownBootstrapping()
        {
            _scope.Dispose();
            _sut = null;
        }

        #endregion

        private TransactionScope _scope;
        private T _sut;

        public T Sut
        {
            get { return _sut ?? (_sut = Resolve<T>()); }
        }

        public TU Resolve<TU>() where TU : class
        {
            return TestBootstrapper.Resolve<TU>();
        }
    }
}