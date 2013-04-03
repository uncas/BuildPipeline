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

        public int AddProject(string projectName)
        {
            const string sql = @"
IF NOT EXISTS (SELECT * FROM Project WHERE ProjectName = @ProjectName)
BEGIN
    INSERT INTO Project (ProjectName) VALUES (@ProjectName)
    SELECT @ProjectId = Scope_Identity()
END
ELSE
BEGIN
    SELECT @ProjectId = ProjectId FROM Project WHERE ProjectName = @ProjectName
END";
            return _connection.ExecuteAndGetGeneratedId(sql,
                                                        new {projectName},
                                                        "ProjectId");
        }

        #endregion
    }
}