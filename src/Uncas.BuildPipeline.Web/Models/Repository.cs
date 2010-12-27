namespace Uncas.BuildPipeline.Web.Models
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public class Repository
    {
        public IEnumerable<Build> GetBuilds(int pageSize)
        {
            var builds = new List<Build>();
            string commandText = string.Format(@"
SELECT TOP {0}
    P.ProjectName
    , B.SourceRevision
    , B.BuildId
FROM Build AS B
JOIN Project AS P
    ON B.ProjectId = P.ProjectId
ORDER BY B.Created DESC",
                pageSize);
            using (DbDataReader reader = GetReader(commandText))
            {
                while (reader.Read())
                {
                    builds.Add(new Build
                    {
                        ProjectName = (string)reader["ProjectName"],
                        SourceRevision = (int)reader["SourceRevision"],
                        Id = (int)reader["BuildId"]
                    });
                }
            }

            AddSteps(builds);
            return builds;
        }

        private void AddSteps(IList<Build> builds)
        {
            foreach (Build build in builds)
            {
                AddSteps(build);
            }
        }

        private static void AddSteps(Build build)
        {
            string commandText = string.Format(@"
SELECT IsSuccessful, StepName
FROM BuildStep
WHERE BuildId = {0}
ORDER BY Created DESC",
                build.Id);
            using (DbDataReader reader = GetReader(commandText))
            {
                while (reader.Read())
                {
                    build.AddStep(new BuildStep
                    {
                        IsSuccessful = (bool)reader["IsSuccessful"],
                        StepName = (string)reader["StepName"],
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