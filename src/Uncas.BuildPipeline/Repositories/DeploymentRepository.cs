namespace Uncas.BuildPipeline.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using Uncas.BuildPipeline.Models;
    using Uncas.Core.Data;
    using Uncas.Core.External;

    public class DeploymentRepository : SQLiteDbContext, IDeploymentRepository
    {
        public DeploymentRepository(
            IBuildPipelineRepositoryConfiguration configuration)
            : base(GetConnectionString(configuration))
        {
        }

        public void AddDeployment(Deployment deployment)
        {
            if (deployment == null)
            {
                throw new ArgumentNullException("deployment");
            }

            const string CommandText =
                @"
INSERT INTO Deployment
(Created, PipelineId, EnvironmentId)
VALUES
(@Created, @PipelineId, @EnvironmentId)

SET @DeploymentId = @@IDENTITY";
            var deploymentIdParameter =
                new SqlParameter("DeploymentId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            using (DbCommand command = CreateCommand())
            {
                command.CommandText = CommandText;
                ModifyData(
                    command,
                    new SqlParameter("Created", deployment.Created),
                    new SqlParameter("PipelineId", deployment.PipelineId),
                    new SqlParameter("EnvironmentId", deployment.EnvironmentId),
                    deploymentIdParameter);
            }

            deployment.ChangeId((int)deploymentIdParameter.Value);
        }

        public IEnumerable<Deployment> GetByEnvironment(int environmentId)
        {
            const string CommandText =
                @"
SELECT DeploymentId
    , PipelineId
    , EnvironmentId
    , Created
    , Started
    , Completed
FROM Deployment
WHERE EnvironmentId = @EnvironmentId
ORDER BY Created ASC";
            var result = new List<Deployment>();
            using (DbCommand command = CreateCommand())
            {
                AddParameter(command, "EnvironmentId", environmentId);
                command.CommandText = CommandText;
                using (DbDataReader reader = GetReader(command))
                {
                    while (reader.Read())
                    {
                        result.Add(MapDataToDeployment(reader));
                    }
                }
            }

            return result.ToList();
        }

        public Deployment GetDeployment(int id)
        {
            const string CommandText =
                @"
SELECT DeploymentId
    , PipelineId
    , EnvironmentId
    , Created
    , Started
    , Completed
FROM Deployment
WHERE DeploymentId = @DeploymentId
ORDER BY Created ASC";
            Deployment deployment = null;
            using (DbCommand command = CreateCommand())
            {
                AddParameter(command, "DeploymentId", id);
                command.CommandText = CommandText;
                using (DbDataReader reader = GetReader(command))
                {
                    if (reader.Read())
                    {
                        deployment = MapDataToDeployment(reader);
                    }
                }
            }

            return deployment;
        }

        public IEnumerable<Deployment> GetDeployments(int pipelineId)
        {
            const string CommandText =
                @"
SELECT DeploymentId
    , PipelineId
    , EnvironmentId
    , Created
    , Started
    , Completed
FROM Deployment
WHERE PipelineId = @PipelineId
ORDER BY Created ASC";

            var result = new List<Deployment>();
            using (DbCommand command = CreateCommand())
            {
                AddParameter(command, "PipelineId", pipelineId);
                command.CommandText = CommandText;
                using (DbDataReader reader = GetReader(command))
                {
                    while (reader.Read())
                    {
                        result.Add(MapDataToDeployment(reader));
                    }
                }
            }

            return result.ToList();
        }

        public IEnumerable<Deployment> GetDueDeployments(
            PagingInfo pagingInfo)
        {
            if (pagingInfo == null)
            {
                throw new ArgumentNullException("pagingInfo");
            }

            const string CommandText =
                @"
SELECT DeploymentId
    , PipelineId
    , EnvironmentId
    , Created
    , Started
    , Completed
FROM Deployment
WHERE Started IS NULL 
    OR Completed IS NULL
ORDER BY Created ASC
LIMIT @PageSize";

            var result = new List<Deployment>();
            using (DbCommand command = CreateCommand())
            {
                command.CommandText = CommandText;
                AddParameter(command, "PageSize", pagingInfo.PageSize);
                using (DbDataReader reader = GetReader(command))
                {
                    while (reader.Read())
                    {
                        result.Add(MapDataToDeployment(reader));
                    }
                }
            }

            return result.ToList();
        }

        public void UpdateDeployment(Deployment deployment)
        {
            if (deployment == null)
            {
                throw new ArgumentNullException("deployment");
            }

            const string CommandText =
                @"
UPDATE Deployment
SET Started = @Started
    , Completed = @Completed
WHERE DeploymentId = @DeploymentId";
            using (DbCommand command = CreateCommand())
            {
                command.CommandText = CommandText;
                AddParameter(command, "Started", deployment.Started);
                AddParameter(command, "Completed", deployment.Completed);
                AddParameter(command, "DeploymentId", deployment.Id);
                ModifyData(command);
            }
        }

        private static string GetConnectionString(
            IBuildPipelineRepositoryConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            return configuration.ConnectionString;
        }

        private static Deployment MapDataToDeployment(DbDataReader reader)
        {
            return Deployment.Reconstruct(
                (int)reader["DeploymentId"],
                (DateTime)reader["Created"],
                (int)reader["PipelineId"],
                (int)reader["EnvironmentId"],
                GetDate(reader, "Started"),
                GetDate(reader, "Completed"));
        }
    }
}