using Uncas.Core.Logging;

namespace Uncas.BuildPipeline.Repositories
{
    public class LogData
    {
        public int ServiceId { get; set; }
        public string Version { get; set; }
        public LogType LogType { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public int? LineNumber { get; set; }
        public string SimpleStackTrace { get; set; }
        public string StackTrace { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string FullException { get; set; }
    }
}