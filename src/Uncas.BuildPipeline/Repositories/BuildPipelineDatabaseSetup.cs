namespace Uncas.BuildPipeline.Repositories
{
    using System.Data.SQLite;
    using Uncas.Core.Data.Migration;

    public static class BuildPipelineDatabaseSetup
    {
        public static void Setup(string connectionString)
        {
            var availableChangeRepository =
                new BuildPipelineSchemaRepository();
            SQLiteFactory factory = SQLiteFactory.Instance;
            var appliedChangeRepository =
                new DbAppliedChangeRepository(
                    factory,
                    connectionString);
            var migrationTarget =
                new DbTarget(
                    factory,
                    connectionString);
            var service = new MigrationService();
            service.Migrate(
                availableChangeRepository,
                appliedChangeRepository,
                migrationTarget);
        }
    }
}