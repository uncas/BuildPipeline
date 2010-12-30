namespace Uncas.BuildPipeline.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public class Repository
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
FROM Pipeline AS Pi
JOIN Project AS Pr
    ON Pi.ProjectId = Pr.ProjectId
ORDER BY Pi.Created DESC",
                pageSize);
            using (DbDataReader reader = GetReader(commandText))
            {
                while (reader.Read())
                {
                    // TODO: Read proper package url here:
                    pipelines.Add(new Pipeline(
                        (int)reader["PipelineId"],
                        (string)reader["ProjectName"],
                        (int)reader["SourceRevision"],
                        (string)reader["SourceUrl"],
                        (string)reader["SourceUrlBase"],
                        (DateTime)reader["Created"],
                        (string)reader["SourceAuthor"],
                        @"C:\Builds\BuildPipeline\Artifacts\Unit\packages\Uncas.BuildPipeline-0.1.36.967.zip"));
                }
            }

            AddSteps(pipelines);
            return pipelines;
        }

        private void AddSteps(IList<Pipeline> pipelines)
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