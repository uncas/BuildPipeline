using System.Collections.Generic;
using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.Repositories
{
    public interface IProjectReadStore
    {
        IEnumerable<ProjectReadModel> GetProjects();
        ProjectReadModel GetProjectById(int projectId);
        void Update(ProjectReadModel project);
        int AddProject(string projectName);
    }
}