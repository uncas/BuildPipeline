using System.Transactions;
using NUnit.Framework;

namespace Uncas.BuildPipeline.Tests.Integration
{
    public abstract class WithBootstrapping<T> where T : class
    {
        private TransactionScope _scope;
        private T _sut;

        public T Sut
        {
            get { return _sut ?? (_sut = Resolve<T>()); }
        }

        #region Setup/Teardown

        [SetUp]
        public void BeforeEach()
        {
            _scope = new TransactionScope();
            _sut = null;
        }

        [TearDown]
        public void AfterEach()
        {
            _scope.Dispose();
            _sut = null;
        }

        #endregion

        public TU Resolve<TU>() where TU : class
        {
            return TestBootstrapper.Resolve<TU>();
        }
    }
}