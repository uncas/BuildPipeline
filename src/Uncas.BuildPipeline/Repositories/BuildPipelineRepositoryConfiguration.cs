namespace Uncas.BuildPipeline.Repositories
{
    using Uncas.Core;

    public class BuildPipelineRepositoryConfiguration :
        IBuildPipelineRepositoryConfiguration
    {
        public string ConnectionString
        {
            get
            {
                return ConfigurationWrapper.GetConnectionString(
                    "BuildPipelineConnectionString");
            }
        }
    }
}