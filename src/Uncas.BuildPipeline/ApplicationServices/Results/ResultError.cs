namespace Uncas.BuildPipeline.ApplicationServices.Results
{
    public class ResultError
    {
        public ResultError(string description)
        {
            Description = description;
        }

        public string Description { get; private set; }
    }
}