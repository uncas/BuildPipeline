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
            : base(configuration.ConnectionString)
        {
        }

        public Pipeline GetPipeline(int pipelineId)
        {
            Pipeline pipeline = null;
            string commandText = string.Format(
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
WHERE Pi.PipelineId = {0}",
                pipelineId);
            using (DbCommand command = CreateCommand())
            {
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
            string commandText = string.Format(
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
LIMIT {0}",
                pageSize);
            using (DbCommand command = CreateCommand())
            {
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
            string commandText = string.Format(
                @"SELECT IsSuccessful, StepName, Created
FROM BuildStep
WHERE PipelineId = {0}
ORDER BY Created ASC",
                pipeline.Id);
            using (DbCommand command = CreateCommand())
            {
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