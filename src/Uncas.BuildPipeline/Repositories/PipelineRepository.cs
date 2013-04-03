﻿using System.Collections.Generic;
using System.Linq;
using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.Repositories
{
    public class PipelineRepository : IPipelineRepository
    {
        private readonly BuildPipelineConnection _connection =
            new BuildPipelineConnection();

        private readonly IProjectReadStore _projectReadStore;

        public PipelineRepository(IProjectReadStore projectReadStore)
        {
            _projectReadStore = projectReadStore;
        }

        #region IPipelineRepository Members

        public Pipeline GetPipeline(int pipelineId)
        {
            const string sql = @"
SELECT Pr.ProjectName
    , Pi.Revision
    , Pi.PipelineId
    , Pi.BranchName
    , Pi.Created
    , Pi.PackagePath
    , Pi.ProjectId
FROM Pipeline AS Pi
JOIN Project AS Pr
    ON Pi.ProjectId = Pr.ProjectId
WHERE Pi.PipelineId = @PipelineId";
            Pipeline pipeline =
                _connection.Query<Pipeline>(sql, new {pipelineId}).SingleOrDefault();
            if (pipeline == null)
                return null;
            AddSteps(pipeline);
            return pipeline;
        }

        public IEnumerable<Pipeline> GetPipelines(int pageSize)
        {
            const string sql = @"
SELECT TOP (@PageSize)
    Pr.ProjectName
    , Pi.Revision
    , Pi.PipelineId
    , Pi.BranchName
    , Pi.Created
    , Pi.PackagePath
    , Pi.ProjectId
FROM Pipeline AS Pi
JOIN Project AS Pr
    ON Pi.ProjectId = Pr.ProjectId
ORDER BY Pi.Created DESC";
            IEnumerable<Pipeline> pipelines = _connection.Query<Pipeline>(sql,
                                                                          new {pageSize});
            AddSteps(pipelines);
            return pipelines;
        }

        public void AddPipeline(Pipeline pipeline)
        {
            int projectId = _projectReadStore.AddProject(pipeline.ProjectName);
            const string sql = @"
SELECT @pipelineId = PipelineId
FROM Pipeline
WHERE ProjectId = @projectId
    AND Revision = @revision
    AND BranchName = @branchName

IF @pipelineId IS NULL
BEGIN
    INSERT INTO Pipeline
    (ProjectId, Revision, BranchName, Created, PackagePath)
    VALUES
    (@projectId, @revision, @branchName, GETDATE(), @packagePath)

    SET @pipelineId = Scope_Identity()
END
ELSE
BEGIN
    UPDATE Pipeline
    SET Modified = GETDATE()
        , PackagePath = @packagePath
    WHERE PipelineId = @pipelineId
END";
            var param =
                new
                    {
                        projectId,
                        pipeline.Revision,
                        pipeline.BranchName,
                        pipeline.PackagePath
                    };
            int pipelineId = _connection.ExecuteAndGetGeneratedId(sql, param, "PipelineId");
            pipeline.AssignId(pipelineId);
        }

        #endregion

        private void AddSteps(IEnumerable<Pipeline> pipelines)
        {
            foreach (Pipeline pipeline in pipelines)
                AddSteps(pipeline);
        }

        private void AddSteps(Pipeline pipeline)
        {
            const string sql = @"
SELECT IsSuccessful, StepName, Created
FROM BuildStep
WHERE PipelineId = @PipelineId
ORDER BY Created ASC";
            var param = new {pipeline.PipelineId};
            IEnumerable<BuildStep> steps = _connection.Query<BuildStep>(sql, param);
            foreach (BuildStep step in steps)
                pipeline.AddStep(step);
        }
    }
}