using System.Collections.Generic;
using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.Repositories
{
    public class CommitRepository : ICommitRepository
    {
        private readonly WithConnection _connection = new BuildPipelineConnection();

        #region ICommitRepository Members

        public IEnumerable<CommitReadModel> GetRevisionsWithoutCommits(
            IEnumerable<int> projectIds)
        {
            const string sql = @"
SELECT P.ProjectId, P.Revision
FROM Pipeline AS P
LEFT JOIN SourceCommit AS C
    ON P.ProjectId = C.ProjectId
    AND P.Revision = C.Revision
WHERE P.ProjectId IN @projectIds
    AND C.ProjectId IS NULL";
            return _connection.Query<CommitReadModel>(sql, new {projectIds});
        }

        public void Add(CommitReadModel commitReadModel)
        {
            const string sql = @"
IF NOT EXISTS (SELECT * FROM SourceCommit WHERE ProjectId = @projectId AND Revision = @revision)
    INSERT INTO SourceCommit
    (ProjectId, Revision, Subject, AuthorName, AuthorEmail, AuthorDate)
    VALUES
    (@projectId, @revision, @subject, @authorName, @authorEmail, @authorDate)
ELSE
    UPDATE SourceCommit
    SET Subject = @subject
        , AuthorName = @authorName
        , AuthorEmail = @authorEmail
        , AuthorDate = @authorDate
    WHERE ProjectId = @projectId
        AND Revision = @revision";
            _connection.Execute(sql, commitReadModel);
        }

        #endregion
    }
}