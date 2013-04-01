using System;
using System.Diagnostics;
using System.Linq;
using Uncas.BuildPipeline.Configuration;
using Uncas.BuildPipeline.Repositories;
using Uncas.Core.Logging;

namespace Uncas.BuildPipeline
{
    public class Logger : ILogger
    {
        #region ILogger Members

        public void Info(string description)
        {
            const int skipFrames = 1;
            var stackTraceWithFileInfo = new StackTrace(skipFrames, true);
            var stackTraceWithoutFileInfo = new StackTrace(skipFrames, false);
            Log(LogType.Info, description, stackTraceWithFileInfo, stackTraceWithoutFileInfo);
        }

        public void Error(Exception exception, string description)
        {
            var stackTraceWithFileInfo = new StackTrace(exception, true);
            var stackTraceWithoutFileInfo = new StackTrace(exception, false);
            string exceptionType = exception.GetType().ToString();
            Log(LogType.Error, description, stackTraceWithFileInfo, stackTraceWithoutFileInfo, exceptionType,
                exception.Message, exception.ToString());
        }

        #endregion

        private void Log(LogType logType, string description, StackTrace withFileInfo, StackTrace withoutFileInfo,
                         string exceptionType = null, string exceptionMessage = null, string fullException = null)
        {
            int serviceId = ConfigurationAppSetting.Int32("ServiceId", 0);
            string version = ApplicationVersion.GetVersion(GetType().Assembly);
            StackFrame stackFrame = withFileInfo.GetFrame(0);
            string relativeFileName = GetFileName(stackFrame);
            int? lineNumber =
                string.IsNullOrWhiteSpace(relativeFileName) ? (int?) null : stackFrame.GetFileLineNumber();
            string stackTrace = withFileInfo.ToString();
            string simpleStackTrace = withoutFileInfo.ToString();
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
            var param = new
                {
                    serviceId,
                    version,
                    logType,
                    description,
                    fileName = relativeFileName,
                    lineNumber,
                    simpleStackTrace,
                    stackTrace,
                    exceptionType,
                    exceptionMessage,
                    fullException
                };
            connection.Execute(sql,
                               param);
        }

        private static string GetFileName(StackFrame stackFrame)
        {
            string fileName = stackFrame.GetFileName();
            if (string.IsNullOrWhiteSpace(fileName))
                return null;
            return fileName.Split(new[] {@"\src\"}, StringSplitOptions.RemoveEmptyEntries).Last();
        }
    }
}