namespace Uncas.BuildPipeline.ApplicationServices.Results
{
    public class ResultError
    {
        public ResultError(string message)
        {
            this.Message = message;
        }

        public string Message { get; private set; }
    }
}