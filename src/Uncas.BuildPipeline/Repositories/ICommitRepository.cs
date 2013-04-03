using System.Collections.Generic;
using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.Repositories
{
    public interface ICommitRepository
    {
        IEnumerable<CommitReadModel> GetRevisionsWithoutCommits(
            IEnumerable<int> projectIds);

        void Add(CommitReadModel commitReadModel);
    }
}