using System;

namespace Uncas.BuildPipeline.Repositories
{
    public class LogRepository : ILogRepository
    {
        #region ILogRepository Members

        public void Add(LogData logData)
        {
            if (logData == null)
                throw new ArgumentNullException();
            var connection = new BuildPipelineConnection();
            const string sql = @"
INSERT INTO LogEntry
(ServiceId, Version, LogType, Description
    , FileName, LineNumber, SimpleStackTrace, StackTrace
    , ExceptionType, ExceptionMessage, FullException)
VALUES
(@ServiceId, @Version, @LogType, @Description
    , @FileName, @LineNumber, @SimpleStackTrace, @StackTrace
    , @ExceptionType, @ExceptionMessage, @FullException)
";
            connection.Execute(sql, logData);
        }

        #endregion
    }
}