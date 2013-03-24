using System;
using System.Collections.Generic;
using System.Linq;

namespace Uncas.BuildPipeline.Models
{
    public class Pipeline
    {
        private readonly IList<BuildStep> _buildSteps;

        private Pipeline()
        {
        }

        public Pipeline(
            int pipelineId,
            string projectName,
            int sourceRevision,
            string sourceUrl,
            string sourceUrlBase,
            DateTime created,
            string sourceAuthor,
            string packagePath)
        {
            _buildSteps = new List<BuildStep>();
            Id = pipelineId;
            ProjectName = projectName;
            SourceRevision = sourceRevision;
            SourceUrl = sourceUrl;
            SourceUrlBase = sourceUrlBase;
            Created = created;
            SourceAuthor = sourceAuthor;
            PackagePath = packagePath;
        }

        public int Id { get; private set; }

        public string ProjectName { get; private set; }

        public string SourceAuthor { get; private set; }

        public int SourceRevision { get; private set; }

        public string SourceUrl { get; private set; }

        public string SourceUrlBase { get; private set; }

        public DateTime Created { get; private set; }

        public string PackagePath { get; private set; }

        public IEnumerable<BuildStep> Steps
        {
            get { return _buildSteps; }
        }

        public bool IsSuccessful
        {
            get { return !Steps.Any(s => !s.IsSuccessful); }
        }

        public void AddStep(BuildStep buildStep)
        {
            // Removing any existing steps with the same name:
            BuildStep existing =
                _buildSteps.FirstOrDefault(s => s.StepName == buildStep.StepName);
            if (existing != null)
            {
                _buildSteps.Remove(existing);
            }

            // Adding the new step:
            _buildSteps.Add(buildStep);
        }
    }
}