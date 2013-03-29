using System;

namespace Uncas.BuildPipeline
{
    public interface ILogger
    {
        void Info(string description);
        void Error(Exception exception, string description);
    }
}