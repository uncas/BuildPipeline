using System.Collections.Generic;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Web.ViewModels;

namespace Uncas.BuildPipeline.Web.QueryServices
{
    public class PipelineQueryService : IPipelineQueryService
    {
        private readonly BuildPipelineConnection _connection =
            new BuildPipelineConnection();

        #region IPipelineQueryService Members

        public IEnumerable<PipelineListItemViewModel> GetPipelines(int pageSize)
        {
            const string sql = @"
SELECT TOP (@PageSize)
    Pi.PipelineId
    , Pi.BranchName
    , Pi.Revision
    , Pi.PackagePath
    , Pi.Created
    , Pr.ProjectId
    , Pr.ProjectName
    , Pr.GithubUrl
    , SC.AuthorName AS SourceAuthor
    , SC.Subject AS SourceSubject
FROM Pipeline AS Pi
JOIN Project AS Pr
    ON Pi.ProjectId = Pr.ProjectId
LEFT JOIN SourceCommit AS SC
    ON Pi.ProjectId = SC.ProjectId
    AND Pi.Revision = SC.Revision
ORDER BY Pi.Created DESC";
            var param = new {pageSize};
            IEnumerable<PipelineListItemViewModel> pipelines = _connection.Query<PipelineListItemViewModel>(sql,
                                                                                                            param);
            AddSteps(pipelines);
            return pipelines;
        }

        #endregion

        private void AddSteps(IEnumerable<PipelineListItemViewModel> pipelines)
        {
            foreach (PipelineListItemViewModel pipeline in pipelines)
                AddSteps(pipeline);
        }

        private void AddSteps(PipelineListItemViewModel pipeline)
        {
            const string sql = @"
SELECT IsSuccessful, StepName, Created
FROM BuildStep
WHERE PipelineId = @PipelineId
ORDER BY Created ASC";
            var param = new {pipeline.PipelineId};
            pipeline.AddSteps(_connection.Query<BuildStep>(sql, param));
        }
    }
}