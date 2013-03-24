using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Uncas.BuildPipeline.Models;

namespace Uncas.BuildPipeline.Repositories
{
    public class PipelineRepository : IPipelineRepository
    {
        private readonly BuildPipelineConnection _connection =
            new BuildPipelineConnection();

        #region IPipelineRepository Members

        public Pipeline GetPipeline(int pipelineId)
        {
            const string commandText = @"
SELECT Pr.ProjectName
    , Pi.SourceRevision
    , Pi.PipelineId
    , Pr.SourceUrlBase
    , Pi.SourceUrl
    , Pi.Created
    , Pi.SourceAuthor
    , Pi.PackagePath
FROM Pipeline AS Pi
JOIN Project AS Pr
    ON Pi.ProjectId = Pr.ProjectId
WHERE Pi.PipelineId = @PipelineId";
            Pipeline pipeline =
                _connection.Query<Pipeline>(commandText, new {pipelineId}).SingleOrDefault
                    ();
            AddSteps(pipeline);
            return pipeline;
        }

        public IEnumerable<Pipeline> GetPipelines(int pageSize)
        {
            const string commandText = @"
SELECT TOP (@PageSize)
    Pr.ProjectName
    , Pi.SourceRevision
    , Pi.PipelineId
    , Pr.SourceUrlBase
    , Pi.SourceUrl
    , Pi.Created
    , Pi.SourceAuthor
    , Pi.PackagePath
FROM Pipeline AS Pi
JOIN Project AS Pr
    ON Pi.ProjectId = Pr.ProjectId
ORDER BY Pi.Created DESC
--LIMIT @PageSize";
            IEnumerable<Pipeline> pipelines = _connection.Query<Pipeline>(commandText,
                                                                          new {pageSize});
            AddSteps(pipelines);
            return pipelines;
        }

        public void AddPipeline(Pipeline pipeline)
        {
            int projectId = AddProject(pipeline.ProjectName, "someUrl");
            const string commandText = @"
INSERT INTO Pipeline
(ProjectId, SourceRevision, SourceUrl, Created, SourceAuthor, PackagePath)
VALUES
(@projectId, @sourceRevision, @sourceUrl, GETDATE(), @sourceAuthor, @packagePath)";
            _connection.Execute(commandText,
                                new
                                    {
                                        PipelineId = pipeline.Id,
                                        projectId,
                                        pipeline.SourceRevision,
                                        pipeline.SourceUrl,
                                        pipeline.SourceAuthor,
                                        pipeline.PackagePath
                                    });
        }

        #endregion

        private int AddProject(string projectName, string sourceUrlBase)
        {
            const string sql = @"
IF NOT EXISTS (SELECT * FROM Project WHERE ProjectName = @ProjectName)
BEGIN
    INSERT INTO Project (ProjectName, SourceUrlBase) VALUES (@ProjectName, @SourceUrlBase)
    SELECT @ProjectId = Scope_Identity()
END
ELSE
BEGIN
    SELECT @ProjectId = ProjectId FROM Project WHERE ProjectName = @ProjectName
END";
            var p = new DynamicParameters();
            p.Add("@projectName", projectName);
            p.Add("@sourceUrlBase", sourceUrlBase);
            p.Add("ProjectId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            _connection.Execute(sql, p);
            return p.Get<int>("ProjectId");
        }

        private void AddSteps(IEnumerable<Pipeline> pipelines)
        {
            foreach (Pipeline pipeline in pipelines)
            {
                AddSteps(pipeline);
            }
        }

        private void AddSteps(Pipeline pipeline)
        {
            const string commandText = @"
SELECT IsSuccessful, StepName, Created
FROM BuildStep
WHERE PipelineId = @PipelineId
ORDER BY Created ASC";
            IEnumerable<BuildStep> steps = _connection.Query<BuildStep>(commandText,
                                                                        new
                                                                            {
                                                                                PipelineId =
                                                                            pipeline.Id
                                                                            });
            foreach (BuildStep step in steps) pipeline.AddStep(step);
        }
    }
}