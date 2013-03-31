using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Commands;

namespace Uncas.BuildPipeline.Tests.Unit.Commands
{
    [TestFixture]
    public class CommandBusTests : WithFixture<CommandBus>
    {
        [Test]
        public void Publish_HandlerNotRegisterd_ThrowsInvalidOperationException()
        {
            Fixture.Register<IServiceLocator>(() => new UnityServiceLocator(new UnityContainer()));

            Assert.Throws<ActivationException>(() => Sut.Publish(Fixture.Create<UpdateGitMirrors>()));
        }

        [Test]
        public void Publish_HandlerRegistered_GetsResolvedOnce()
        {
            var mock = new Mock<IServiceLocator>();
            mock.Setup(x => x.GetInstance(typeof (UpdateGitMirrorsHandler))).Returns(
                Fixture.Create<UpdateGitMirrorsHandler>());
            Fixture.Register(() => mock.Object);

            Sut.Publish(Fixture.Create<UpdateGitMirrors>());

            mock.Verify(x => x.GetInstance(typeof (UpdateGitMirrorsHandler)), Times.Once());
        }
    }
}