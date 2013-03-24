using Uncas.Core;

namespace Uncas.BuildPipeline.Repositories
{
    public class BuildPipelineRepositoryConfiguration :
        IBuildPipelineRepositoryConfiguration
    {
        #region IBuildPipelineRepositoryConfiguration Members

        public string ConnectionString
        {
            get
            {
                return
                    ConfigurationWrapper.GetConnectionString(
                        "BuildPipelineConnectionString",
                        "Server=.\\SqlExpress;Database=BuildPipeline;Integrated Security=true;");
            }
        }

        #endregion
    }
}