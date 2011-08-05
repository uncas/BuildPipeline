namespace Uncas.BuildPipeline.Repositories
{
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.IO;
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

    public class BuildPipelineSchemaRepository : IAvailableChangeRepository<DbChange>
    {
        /// <summary>
        /// Gets the available changes.
        /// </summary>
        /// <returns>A list of changes.</returns>
        public IEnumerable<DbChange> GetAvailableChanges()
        {
            var result = new List<DbChange>();
            result.Add(GetChange("01-Project"));
            result.Add(GetChange("02-Pipeline"));
            result.Add(GetChange("03-BuildStep"));
            result.Add(GetChange("04-Deployment"));
            return result;
        }

        private static DbChange GetChange(string scriptName)
        {
            return new DbChange(scriptName, ReadScript(scriptName + ".sql"));
        }

        private static string ReadScript(string scriptName)
        {
            string path = Path.Combine("Repositories\\SQLiteScripts", scriptName);
            return File.ReadAllText(path);
        }
    }
}