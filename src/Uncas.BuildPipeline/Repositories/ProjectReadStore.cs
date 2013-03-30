using System.Collections.Generic;

namespace Uncas.BuildPipeline.Repositories
{
    public class ProjectReadStore : IProjectReadStore
    {
        #region IProjectReadStore Members

        public IEnumerable<ProjectReadModel> GetProjects()
        {
            var connection = new BuildPipelineConnection();
            return connection.Query<ProjectReadModel>("SELECT ProjectName, GitRemoteUrl FROM Project");
        }

        #endregion
    }
}