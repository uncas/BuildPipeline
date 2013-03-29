using System;
using Microsoft.Practices.ServiceLocation;
using Moq;
using NUnit.Framework;
using Uncas.BuildPipeline.ApplicationServices;
using Uncas.BuildPipeline.Commands;

namespace Uncas.BuildPipeline.Tests.Unit
{
    [TestFixture]
    public class CommandBusTests
    {
        [Test]
        public void Publish_HandlerNotRegisterd_ThrowsInvalidOperationException()
        {
            var mock = new Mock<IServiceLocator>();
            var bus = new CommandBus(mock.Object);

            Assert.Throws<InvalidOperationException>(() => bus.Publish(new UpdateGitMirrors()));
        }

        [Test]
        public void Publish_HandlerRegistered_GetsResolvedOnce()
        {
            var mock = new Mock<IServiceLocator>();
            mock.Setup(x => x.GetInstance(typeof (UpdateGitMirrorsHandler))).Returns(new UpdateGitMirrorsHandler());
            var bus = new CommandBus(mock.Object);

            bus.Publish(new UpdateGitMirrors());

            mock.Verify(x => x.GetInstance(typeof (UpdateGitMirrorsHandler)), Times.Once());
        }
    }
}