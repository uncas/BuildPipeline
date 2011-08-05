namespace Uncas.BuildPipeline.Tests.Integration.Repositories
{
    using NUnit.Framework;
    using Uncas.BuildPipeline.Repositories;

    [TestFixture]
    public class BuildPipelineDatabaseSetupTests
    {
        [Test]
        public void Setup_ValidConnection_RunsWithoutErrors()
        {
            BuildPipelineDatabaseSetup.Setup("Data Source=Test.db;Version=3;");
        }
    }
}