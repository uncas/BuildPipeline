using System.Collections.Generic;
using NUnit.Framework;

namespace Uncas.BuildPipeline.Tests.Unit
{
    [TestFixture]
    public class MaybeTests
    {
        [Test]
        public void EmptyList_StringLength_FallsBackToZero()
        {
            IList<string> texts = new List<string>();

            int length = texts.Maybe<string, int>(x => x.Length);

            Assert.AreEqual(0, length);
        }

        [Test]
        public void NullList_StringLength_FallsBackToZero()
        {
            IList<string> texts = null;

            int length = texts.Maybe<string, int>(x => x.Length);

            Assert.AreEqual(0, length);
        }

        [Test]
        public void NullString_Length_FallsBackToZero()
        {
            string text = null;

            int length = text.Maybe(x => x.Length);

            Assert.AreEqual(0, length);
        }
    }
}