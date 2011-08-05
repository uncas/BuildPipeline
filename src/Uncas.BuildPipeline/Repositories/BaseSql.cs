namespace Uncas.BuildPipeline.Repositories
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public abstract class BaseSql
    {
        private readonly string _connectionString;

        protected BaseSql(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected internal static SqlParameter GetDateTimeParameter(
            string name,
            DateTime? value)
        {
            var result = new SqlParameter(name, SqlDbType.DateTime);
            if (value.HasValue)
            {
                result.Value = value.Value;
            }
            else
            {
                result.Value = DBNull.Value;
            }
            return result;
        }

        protected internal static DateTime? GetDateTimeValue(
            object dbValue)
        {
            if (dbValue == null || dbValue is DBNull)
            {
                return null;
            }
            return (DateTime)dbValue;
        }

        protected internal static string GetStringValue(object dbValue)
        {
            if (dbValue == null || dbValue is DBNull)
            {
                return string.Empty;
            }
            return (string)dbValue;
        }

        protected internal void ExecuteNonQuery(
            string commandText,
            params SqlParameter[] parameters)
        {
            using (var connection =
                new SqlConnection(_connectionString))
            {
                using (var command =
                    new SqlCommand(commandText, connection))
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        protected internal SqlDataReader GetReader(string commandText)
        {
            var connection =
                new SqlConnection(_connectionString);
            using (var command =
                new SqlCommand(commandText, connection))
            {
                connection.Open();
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }
    }
}