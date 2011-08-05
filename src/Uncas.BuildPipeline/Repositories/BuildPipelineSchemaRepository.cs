namespace Uncas.BuildPipeline.Repositories
{
    using System.Collections.Generic;
    using Uncas.Core.Data.Migration;

    public class BuildPipelineSchemaRepository :
        IAvailableChangeRepository<DbChange>
    {
        private const string Project =
            @"
CREATE TABLE Project
(
    ProjectId    integer    NOT NULL
        PRIMARY KEY ASC
    , ProjectName    text    NOT NULL
        UNIQUE
    , SourceUrlBase    text    NOT NULL
)";

        private const string Pipeline =
            @"
CREATE TABLE Pipeline
(
    PipelineId    integer    NOT NULL
        PRIMARY KEY ASC
    , ProjectId    integer    NOT NULL
    , SourceUrl    text    NOT NULL
    , SourceRevision    integer    NOT NULL
    , Created    datetime    NOT NULL
    , Modified    datetime    NOT NULL
    , SourceAuthor    text    NOT NULL
    , PackagePath    text    NULL
    , FOREIGN KEY (ProjectId) REFERENCES Project (ProjectId)
)";

        private const string BuildStep =
            @"CREATE TABLE BuildStep
(
    BuildStepId    integer    NOT NULL
        PRIMARY KEY ASC
    , IsSuccessful    integer    NOT NULL
    , StepName    text    NOT NULL
    , BuildNumber    integer    NOT NULL
    , Created    datetime    NOT NULL
    , PipelineId    int    NOT NULL
    , FOREIGN KEY (PipelineId) REFERENCES Pipeline (PipelineId)
)";

        private const string Deployment =
            @"CREATE TABLE Deployment
(
    DeploymentId    integer    NOT NULL
        PRIMARY KEY ASC
    , Created    datetime    NOT NULL
    , PipelineId    integer    NOT NULL
    , EnvironmentId    integer    NOT NULL
    , Started    datetime    NULL
    , Completed    datetime    NULL
    , FOREIGN KEY (PipelineId) REFERENCES Pipeline (PipelineId)
 )";

        /// <summary>
        /// Gets the available changes.
        /// </summary>
        /// <returns>A list of changes.</returns>
        public IEnumerable<DbChange> GetAvailableChanges()
        {
            var result = new List<DbChange>();
            result.Add(GetChange("01-Project", Project));
            result.Add(GetChange("02-Pipeline", Pipeline));
            result.Add(GetChange("03-BuildStep", BuildStep));
            result.Add(GetChange("04-Deployment", Deployment));
            return result;
        }

        private static DbChange GetChange(string scriptName, string content)
        {
            return new DbChange(scriptName, content);
        }
    }
}