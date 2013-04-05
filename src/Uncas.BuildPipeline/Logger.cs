using System;
using System.Diagnostics;
using System.Linq;
using Uncas.BuildPipeline.Configuration;
using Uncas.BuildPipeline.Repositories;
using Uncas.Core.Logging;
using ILogRepository = Uncas.BuildPipeline.Repositories.ILogRepository;

namespace Uncas.BuildPipeline
{
    public class Logger : ILogger
    {
        private readonly ILogRepository _logRepository;

        public Logger(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        #region ILogger Members

        public void Debug(string descriptionFormat, params object[] args)
        {
            LogNoException(LogType.Debug, 2, descriptionFormat, args);
        }

        public void Info(string descriptionFormat, params object[] args)
        {
            LogNoException(LogType.Info, 2, descriptionFormat, args);
        }

        public void Error(string descriptionFormat, params object[] args)
        {
            LogNoException(LogType.Error, 2, descriptionFormat, args);
        }

        public void Error(Exception exception, string descriptionFormat,
                          params object[] args)
        {
            var stackTraceWithFileInfo = new StackTrace(exception, true);
            var stackTraceWithoutFileInfo = new StackTrace(exception, false);
            string exceptionType = exception.GetType().ToString();
            Log(LogType.Error, GetDescription(descriptionFormat, args),
                stackTraceWithFileInfo,
                stackTraceWithoutFileInfo, exceptionType,
                exception.Message, exception.ToString());
        }

        #endregion

        private void LogNoException(LogType logType, int skipFrames,
                                    string descriptionFormat, object[] args)
        {
            var stackTraceWithFileInfo = new StackTrace(skipFrames, true);
            var stackTraceWithoutFileInfo = new StackTrace(skipFrames, false);
            Log(logType, GetDescription(descriptionFormat, args), stackTraceWithFileInfo,
                stackTraceWithoutFileInfo);
        }

        private static string GetDescription(string descriptionFormat,
                                             params object[] args)
        {
            return string.Format(descriptionFormat, args);
        }

        private void Log(LogType logType, string description, StackTrace withFileInfo,
                         StackTrace withoutFileInfo,
                         string exceptionType = null, string exceptionMessage = null,
                         string fullException = null)
        {
            int serviceId = ConfigurationAppSetting.Int32("ServiceId", 0);
            string version = ApplicationVersion.GetVersion(GetType().Assembly);
            StackFrame stackFrame = withFileInfo.GetFrame(0);
            string fileName = GetFileName(stackFrame);
            int? lineNumber =
                string.IsNullOrWhiteSpace(fileName)
                    ? (int?) null
                    : stackFrame.GetFileLineNumber();
            string stackTrace = withFileInfo.ToString();
            string simpleStackTrace = withoutFileInfo.ToString();
            var logData = new LogData
                {
                    ServiceId = serviceId,
                    Version = version,
                    LogType = logType,
                    Description = description,
                    FileName = fileName,
                    LineNumber = lineNumber,
                    SimpleStackTrace = simpleStackTrace,
                    StackTrace = stackTrace,
                    ExceptionType = exceptionType,
                    ExceptionMessage = exceptionMessage,
                    FullException = fullException
                };
            _logRepository.Add(logData);
        }

        private static string GetFileName(StackFrame stackFrame)
        {
            if (stackFrame == null)
                return null;
            string fileName = stackFrame.GetFileName();
            if (string.IsNullOrWhiteSpace(fileName))
                return null;
            return
                fileName.Split(new[] {@"\src\"}, StringSplitOptions.RemoveEmptyEntries).
                    Last();
        }
    }
}