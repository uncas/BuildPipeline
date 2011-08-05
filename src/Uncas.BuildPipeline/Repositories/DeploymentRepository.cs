namespace Uncas.BuildPipeline.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using Uncas.BuildPipeline.Models;
    using Uncas.Core.External;

    public class DeploymentRepository : SQLiteDbContext, IDeploymentRepository
    {
        public DeploymentRepository(
            IBuildPipelineRepositoryConfiguration configuration)
            : base(configuration.ConnectionString)
        {
        }

        public void AddDeployment(Deployment deployment)
        {
            if (deployment == null)
            {
                throw new ArgumentNullException("deployment");
            }

            const string commandText =
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
                command.CommandText = commandText;
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
            string commandText = string.Format(
                @"SELECT DeploymentId
    , PipelineId
    , EnvironmentId
    , Created
    , Started
    , Completed
FROM Deployment
WHERE EnvironmentId = {0}
ORDER BY Created ASC",
                environmentId);
            var result = new List<Deployment>();
            using (DbCommand command = CreateCommand())
            {
                command.CommandText = commandText;
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
            string commandText = string.Format(
                @"SELECT DeploymentId
    , PipelineId
    , EnvironmentId
    , Created
    , Started
    , Completed
FROM Deployment
WHERE DeploymentId = {0}
ORDER BY Created ASC",
                id);
            Deployment deployment = null;
            using (DbCommand command = CreateCommand())
            {
                command.CommandText = commandText;
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
            string commandText =
                string.Format(
                    @"SELECT DeploymentId
    , PipelineId
    , EnvironmentId
    , Created
    , Started
    , Completed
FROM Deployment
WHERE PipelineId = {0}
ORDER BY Created ASC",
                    pipelineId);

            var result = new List<Deployment>();
            using (DbCommand command = CreateCommand())
            {
                command.CommandText = commandText;
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

        public IEnumerable<Deployment> GetDueDeployments()
        {
            const string commandText =
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
ORDER BY Created ASC";

            var result = new List<Deployment>();
            using (DbCommand command = CreateCommand())
            {
                command.CommandText = commandText;
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

            const string commandText =
                @"
UPDATE Deployment
SET Started = @Started
    , Completed = @Completed
WHERE DeploymentId = @DeploymentId";
            using (DbCommand command = CreateCommand())
            {
                command.CommandText = commandText;
                AddParameter(command, "Started", deployment.Started);
                AddParameter(command, "Completed", deployment.Completed);
                AddParameter(command, "DeploymentId", deployment.Id);
                ModifyData(command);
            }
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