using System.Collections.Generic;
using System.Linq;

namespace Uncas.BuildPipeline.Repositories
{
    public class ProjectReadStore : IProjectReadStore
    {
        private readonly WithConnection _connection = new BuildPipelineConnection();

        #region IProjectReadStore Members

        public IEnumerable<ProjectReadModel> GetProjects()
        {
            const string sql = @"
SELECT ProjectId, ProjectName, GitRemoteUrl, GithubUrl, DeploymentScript
FROM Project";
            return
                _connection.Query<ProjectReadModel>(
                    sql);
        }

        public ProjectReadModel GetProjectById(int projectId)
        {
            const string sql = @"
SELECT ProjectId, ProjectName, GitRemoteUrl, GithubUrl, DeploymentScript
FROM Project
WHERE ProjectId = @projectId";
            return
                _connection.Query<ProjectReadModel>(
                    sql, new {projectId}).
                    SingleOrDefault();
        }

        public void Update(ProjectReadModel project)
        {
            const string sql = @"
UPDATE Project SET GitRemoteUrl = @gitRemoteUrl
    , GithubUrl = @githubUrl
    , DeploymentScript = @deploymentScript
WHERE ProjectId = @projectId";
            _connection.Execute(sql, project);
        }

        #endregion
    }
}