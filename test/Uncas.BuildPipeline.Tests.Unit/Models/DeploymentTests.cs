using System;
using NUnit.Framework;
using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.Tests.Unit.Models
{
    public class DeploymentTests : WithFixture<Deployment>
    {
        [Test]
        public void MarkAsCompleted_NotStarted_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => Sut.MarkAsCompleted());
        }

        [Test]
        public void MarkAsCompleted_Started_MarksAsCompleted()
        {
            Sut.MarkAsStarted();

            Sut.MarkAsCompleted();

            Assert.That(Sut.Completed.HasValue);
        }
    }
}