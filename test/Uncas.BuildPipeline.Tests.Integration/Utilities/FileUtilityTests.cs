using NUnit.Framework;
using Ploeh.AutoFixture;
using Uncas.BuildPipeline.Tests.Unit;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Tests.Integration.Utilities
{
    public class FileUtilityTests : WithFixture<FileUtility>
    {
        [Test]
        public void FileExists()
        {
            Sut.FileExists(Fixture.Create<string>());
        }

        [Test]
        public void WriteAllText()
        {
            Sut.WriteAllText(@"C:\Temp\testkasasjdasjdlaskj.txt", Fixture.Create<string>());
        }
    }
}