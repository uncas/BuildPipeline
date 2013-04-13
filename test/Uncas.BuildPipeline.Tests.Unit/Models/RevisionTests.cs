using NUnit.Framework;
using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.Tests.Unit.Models
{
    public class RevisionTests : WithFixture<Revision>
    {
        [Test]
        public void Short()
        {
            string shortRevision = Revision.Short("qæwlekælksadælkqæwlke");

            StringAssert.AreEqualIgnoringCase("qæwlekælks", shortRevision);
        }
    }
}