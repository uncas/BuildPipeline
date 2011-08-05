namespace Uncas.BuildPipeline.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using Uncas.BuildPipeline.Models;
    using Uncas.Core.External;

    public class PipelineRepository : SQLiteDbContext, IPipelineRepository
    {
        public PipelineRepository(
            IBuildPipelineRepositoryConfiguration configuration)
            : base(GetConnectionString(configuration))
        {
        }

        public Pipeline GetPipeline(int pipelineId)
        {
            Pipeline pipeline = null;
            const string commandText =
                @"
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
                command.CommandText = commandText;
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
            const string commandText =
                @"
SELECT
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
LIMIT @PageSize";
            using (DbCommand command = CreateCommand())
            {
                AddParameter(command, "PageSize", pageSize);
                command.CommandText = commandText;
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
            return new Pipeline(
                (int)reader["PipelineId"],
                (string)reader["ProjectName"],
                (int)reader["SourceRevision"],
                (string)reader["SourceUrl"],
                (string)reader["SourceUrlBase"],
                (DateTime)reader["Created"],
                (string)reader["SourceAuthor"],
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
            const string commandText =
                @"
SELECT IsSuccessful, StepName, Created
FROM BuildStep
WHERE PipelineId = @PipelineId
ORDER BY Created ASC";
            using (DbCommand command = CreateCommand())
            {
                AddParameter(command, "PipelineId", pipeline.Id);
                command.CommandText = commandText;
                using (DbDataReader reader = GetReader(command))
                {
                    while (reader.Read())
                    {
                        pipeline.AddStep(new BuildStep(
                                             (bool)reader["IsSuccessful"],
                                             (string)reader["StepName"],
                                             (DateTime)reader["Created"]));
                    }
                }
            }
        }
    }
}