﻿namespace Uncas.BuildPipeline.Web.Models
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
    P.ProjectName
    , B.SourceRevision
    , B.BuildId
    , P.SourceUrlBase
    , B.SourceUrl
    , B.Created
FROM Build AS B
JOIN Project AS P
    ON B.ProjectId = P.ProjectId
ORDER BY B.Created DESC",
                pageSize);
            using (DbDataReader reader = GetReader(commandText))
            {
                while (reader.Read())
                {
                    pipelines.Add(new Pipeline
                    {
                        ProjectName = (string)reader["ProjectName"],
                        SourceRevision = (int)reader["SourceRevision"],
                        Id = (int)reader["BuildId"],
                        SourceUrl = (string)reader["SourceUrl"],
                        SourceUrlBase = (string)reader["SourceUrlBase"],
                        Created = (DateTime)reader["Created"]
                    });
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
WHERE BuildId = {0}
ORDER BY Created ASC",
                pipeline.Id);
            using (DbDataReader reader = GetReader(commandText))
            {
                while (reader.Read())
                {
                    pipeline.AddStep(new BuildStep
                    {
                        IsSuccessful = (bool)reader["IsSuccessful"],
                        StepName = (string)reader["StepName"],
                        Created = (DateTime)reader["Created"]
                    });
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