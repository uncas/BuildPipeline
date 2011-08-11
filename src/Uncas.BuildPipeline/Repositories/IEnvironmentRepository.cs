namespace Uncas.BuildPipeline.Repositories
{
    using System.Collections.Generic;
    using Uncas.BuildPipeline.Models;

    public interface IEnvironmentRepository
    {
        Environment GetEnvironment(int environmentId);
        IEnumerable<Environment> GetEnvironments(PagingInfo pagingInfo);
    }
}