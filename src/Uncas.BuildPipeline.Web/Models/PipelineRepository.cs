namespace Uncas.BuildPipeline.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public class PipelineRepository
    {
        public IEnumerable<Pipeline> GetPipelines(int pageSize)
        {
            var pipelines = new List<Pipeline>();
            string commandText = string.Format(@"
SELECT TOP {0}
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
ORDER BY Pi.Created DESC",
                pageSize);
            using (DbDataReader reader = GetReader(commandText))
            {
                while (reader.Read())
                {
                    pipelines.Add(MapDataToPipeline(reader));
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
                GetStringValue(reader["PackagePath"]));
        }

        public Pipeline GetPipeline(int pipelineId)
        {
            Pipeline pipeline=null;
            string commandText = string.Format(@"
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
            using (DbDataReader reader = GetReader(commandText))
            {
                if (reader.Read())
                {
                    pipeline = MapDataToPipeline(reader);
                }
            }

            AddSteps(pipeline);
            return pipeline;
        }

        private static string GetStringValue(object dbValue)
        {
            if (dbValue == null || dbValue is DBNull)
                return string.Empty;
            return (string)dbValue;
        }

        private static void AddSteps(IList<Pipeline> pipelines)
        {
            foreach (Pipeline pipeline in pipelines)
            {
                AddSteps(pipeline);
            }
        }

        private static void AddSteps(Pipeline pipeline)
        {
            string commandText = string.Format(@"
SELECT IsSuccessful, StepName, Created
FROM BuildStep
WHERE PipelineId = {0}
ORDER BY Created ASC",
                pipeline.Id);
            using (DbDataReader reader = GetReader(commandText))
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

        private static SqlDataReader GetReader(string commandText)
        {
            string connectionString =
                @"Server=.\SqlExpress;Database=BuildPipeline;User Id=sa;Pwd=ols";
            var connection =
                new SqlConnection(connectionString);
            using (var command =
                new SqlCommand(commandText, connection))
            {
                connection.Open();
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }
    }
}