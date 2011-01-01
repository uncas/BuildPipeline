﻿namespace Uncas.BuildPipeline.Repositories
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public abstract class BaseSql
    {
        private string connectionString;

        protected BaseSql(string connectionString)
        {
            this.connectionString = connectionString;
        }

        internal protected static DateTime? GetDateTimeValue(
            object dbValue)
        {
            if (dbValue == null || dbValue is DBNull)
                return null;
            return (DateTime)dbValue;
        }

        internal protected static string GetStringValue(object dbValue)
        {
            if (dbValue == null || dbValue is DBNull)
                return string.Empty;
            return (string)dbValue;
        }

        internal protected SqlDataReader GetReader(string commandText)
        {
            var connection =
                new SqlConnection(this.connectionString);
            using (var command =
                new SqlCommand(commandText, connection))
            {
                connection.Open();
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        internal protected void ExecuteNonQuery(
            string commandText,
            params SqlParameter[] parameters)
        {
            using (var connection =
                new SqlConnection(this.connectionString))
            {
                using (var command =
                    new SqlCommand(commandText, connection))
                {
                    foreach (var parameter in parameters)
                        command.Parameters.Add(parameter);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        internal protected static SqlParameter GetDateTimeParameter(
            string name,
            DateTime? value)
        {
            var result = new SqlParameter(name, SqlDbType.DateTime);
            if (value.HasValue)
                result.Value = value.Value;
            else
                result.Value = DBNull.Value;
            return result;
        }
    }
}