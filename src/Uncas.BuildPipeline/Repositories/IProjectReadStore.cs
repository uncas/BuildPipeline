using System.Collections.Generic;

namespace Uncas.BuildPipeline.Repositories
{
    public interface IProjectReadStore
    {
        IEnumerable<ProjectReadModel> GetProjects();
        ProjectReadModel GetProjectById(int projectId);
        void Update(ProjectReadModel project);
    }
}