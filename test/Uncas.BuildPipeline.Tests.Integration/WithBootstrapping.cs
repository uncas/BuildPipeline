using NUnit.Framework;

namespace Uncas.BuildPipeline.Tests.Integration
{
    public abstract class WithBootstrapping<T> where T : class
    {
        private T _sut;

        public T Sut
        {
            get { return _sut ?? (_sut = SharedBootstrapper.Resolve<T>()); }
        }

        [SetUp]
        public virtual void BeforeEach()
        {
            _sut = null;
        }
    }
}