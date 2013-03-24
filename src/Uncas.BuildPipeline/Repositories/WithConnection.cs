using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using Dapper;

namespace Uncas.BuildPipeline.Repositories
{
    [DebuggerDisplay("{ConnectionString}")]
    public class WithConnection
    {
        private readonly Lazy<string> _connectionString;

        public WithConnection(string connectionString) : this(() => connectionString)
        {
        }

        public WithConnection(Func<string> connectionStringFetcher)
        {
            _connectionString = new Lazy<string>(connectionStringFetcher);
        }

        public string ConnectionString
        {
            get { return _connectionString.Value; }
        }

        public IEnumerable<dynamic> Query(string sql, dynamic param = null)
        {
            using (DbConnection conn = Open())
                return SqlMapper.Query(conn, sql, param);
        }

        public virtual IEnumerable<TResult> Query<TResult>(
            string sql, dynamic param = null)
        {
            using (DbConnection conn = Open())
            {
                return SqlMapper.Query<TResult>(conn, sql, param);
            }
        }

        public int Execute(
            string sql,
            dynamic param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            using (DbConnection conn = Open())
            {
                return SqlMapper.Execute(conn,
                                         sql,
                                         param,
                                         transaction,
                                         commandTimeout,
                                         commandType);
            }
        }

        public TResult Execute<TResult>(
            Func<DbConnection, DbTransaction, TResult> task,
            IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted)
        {
            if (task == null) throw new ArgumentNullException("task");
            using (DbConnection conn = Open())
            using (DbTransaction tran = conn.BeginTransaction(isolationLevel))
            {
                try
                {
                    TResult result = task(conn, tran);
                    tran.Commit();
                    return result;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        public DbConnection Open()
        {
            return GetConnection(true, _connectionString.Value);
        }

        private static DbConnection GetConnection(bool opened, string connectionString)
        {
            DbConnection dbConnection = SqlClientFactory.Instance.CreateConnection();
            dbConnection.ConnectionString = connectionString;
            if (opened)
                dbConnection.Open();
            return dbConnection;
        }
    }
}