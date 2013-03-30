using System.Collections.Generic;

namespace Uncas.BuildPipeline.Repositories
{
    public interface IProjectReadStore
    {
        IEnumerable<ProjectReadModel> GetProjects();
    }
}