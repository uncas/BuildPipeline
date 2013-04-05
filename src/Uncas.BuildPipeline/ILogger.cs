using System;

namespace Uncas.BuildPipeline
{
    public interface ILogger
    {
        void Debug(string descriptionFormat, params object[] args);
        void Info(string descriptionFormat, params object[] args);
        void Error(string descriptionFormat, params object[] args);
        void Error(Exception exception, string descriptionFormat, params object[] args);
    }
}