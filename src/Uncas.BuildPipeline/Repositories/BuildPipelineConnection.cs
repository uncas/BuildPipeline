using Uncas.Core;

namespace Uncas.BuildPipeline.Repositories
{
    public class BuildPipelineConnection : WithConnection
    {
        public BuildPipelineConnection()
            : base(
                () =>
                ConfigurationWrapper.GetConnectionString("BuildPipelineConnectionString",
                                                         "Server=.\\SqlExpress;Database=BuildPipeline;Integrated Security=true;")
                )
        {
        }
    }
}