using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Uncas.BuildPipeline.Models;
using Uncas.Core.Data;

namespace Uncas.BuildPipeline.Repositories
{
    public class PipelineRepository : SqlDbContext, IPipelineRepository
    {
        public PipelineRepository(IBuildPipelineRepositoryConfiguration configuration)
            : base(GetConnectionString(configuration))
        {
        }

        #region IPipelineRepository Members

        public Pipeline GetPipeline(int pipelineId)
        {
            Pipeline pipeline = null;
            const string CommandText = @"
SELECT Pr.ProjectName
    , Pi.SourceRevision
    , Pi.PipelineId
    , Pr.SourceUrlBase
    , Pi.SourceUrl
    , Pi.Created
    , Pi.SourceAuthor
    , Pi.PackagePath
FROM Pipeline AS Pi
JOIN Project AS Pr
    ON Pi.ProjectId = Pr.ProjectId
WHERE Pi.PipelineId = @PipelineId";
            using (DbCommand command = CreateCommand())
            {
                AddParameter(command, "PipelineId", pipelineId);
                command.CommandText = CommandText;
                using (DbDataReader reader = GetReader(command))
                {
                    if (reader.Read())
                    {
                        pipeline = MapDataToPipeline(reader);
                    }
                }
            }

            AddSteps(pipeline);
            return pipeline;
        }

        public IEnumerable<Pipeline> GetPipelines(int pageSize)
        {
            var pipelines = new List<Pipeline>();
            const string CommandText = @"
SELECT TOP (@PageSize)
    Pr.ProjectName
    , Pi.SourceRevision
    , Pi.PipelineId
    , Pr.SourceUrlBase
    , Pi.SourceUrl
    , Pi.Created
    , Pi.SourceAuthor
    , Pi.PackagePath
FROM Pipeline AS Pi
JOIN Project AS Pr
    ON Pi.ProjectId = Pr.ProjectId
ORDER BY Pi.Created DESC
--LIMIT @PageSize";
            using (DbCommand command = CreateCommand())
            {
                AddParameter(command, "PageSize", pageSize);
                command.CommandText = CommandText;
                using (DbDataReader reader = GetReader(command))
                {
                    while (reader.Read())
                    {
                        pipelines.Add(MapDataToPipeline(reader));
                    }
                }
            }

            AddSteps(pipelines);
            return pipelines;
        }

        public void AddPipeline(Pipeline pipeline)
        {
            int projectId = AddProject(pipeline.ProjectName, "someUrl");
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                AddParameter(command, "PipelineId", pipeline.Id);
                command.CommandText = @"
INSERT INTO Pipeline
(ProjectId, SourceRevision, SourceUrl, Created, SourceAuthor, PackagePath)
VALUES
(@projectId, @sourceRevision, @sourceUrl, GETDATE(), @sourceAuthor, @packagePath)";
                command.Parameters.AddWithValue("ProjectId", projectId);
                command.Parameters.AddWithValue("sourceRevision", pipeline.SourceRevision);
                command.Parameters.AddWithValue("SourceUrl", pipeline.SourceUrl);
                command.Parameters.AddWithValue("SourceAuthor", pipeline.SourceAuthor);
                command.Parameters.AddWithValue("PackagePath", pipeline.PackagePath);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
        }

        #endregion

        private int AddProject(string projectName, string sourceUrlBase)
        {
            const string sql = @"
IF NOT EXISTS (SELECT * FROM Project WHERE ProjectName = @ProjectName)
BEGIN
    INSERT INTO Project (ProjectName, SourceUrlBase) VALUES (@ProjectName, @SourceUrlBase)
    SELECT @ProjectId = Scope_Identity()
END
ELSE
BEGIN
    SELECT @ProjectId = ProjectId FROM Project WHERE ProjectName = @ProjectName
END";
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = sql;
                command.Parameters.AddWithValue("ProjectName", projectName);
                command.Parameters.AddWithValue("SourceUrlBase", sourceUrlBase);
                var id = new SqlParameter("ProjectId", SqlDbType.Int)
                    {Direction = ParameterDirection.Output};
                command.Parameters.Add(id);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
                return (int) id.Value;
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

        private static Pipeline MapDataToPipeline(DbDataReader reader)
        {
            return new Pipeline((int) reader["PipelineId"],
                                (string) reader["ProjectName"],
                                (int) reader["SourceRevision"],
                                (string) reader["SourceUrl"],
                                (string) reader["SourceUrlBase"],
                                (DateTime) reader["Created"],
                                (string) reader["SourceAuthor"],
                                GetString(reader, "PackagePath"));
        }

        private void AddSteps(IEnumerable<Pipeline> pipelines)
        {
            foreach (Pipeline pipeline in pipelines)
            {
                AddSteps(pipeline);
            }
        }

        private void AddSteps(Pipeline pipeline)
        {
            const string CommandText = @"
SELECT IsSuccessful, StepName, Created
FROM BuildStep
WHERE PipelineId = @PipelineId
ORDER BY Created ASC";
            using (DbCommand command = CreateCommand())
            {
                AddParameter(command, "PipelineId", pipeline.Id);
                command.CommandText = CommandText;
                using (DbDataReader reader = GetReader(command))
                {
                    while (reader.Read())
                    {
                        pipeline.AddStep(new BuildStep((bool) reader["IsSuccessful"],
                                                       (string) reader["StepName"],
                                                       (DateTime) reader["Created"]));
                    }
                }
            }
        }
    }
}