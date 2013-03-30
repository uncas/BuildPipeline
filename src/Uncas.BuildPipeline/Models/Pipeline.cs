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
            _buildSteps = new List<BuildStep>();
        }

        public Pipeline(
            int pipelineId,
            string projectName,
            string revision,
            string branchName,
            DateTime created,
            string sourceAuthor,
            string packagePath) : this()
        {
            PipelineId = pipelineId;
            ProjectName = projectName;
            Revision = revision;
            BranchName = branchName;
            Created = created;
            SourceAuthor = sourceAuthor;
            PackagePath = packagePath;
        }

        public int PipelineId { get; private set; }
        public string ProjectName { get; private set; }
        public string Revision { get; private set; }
        public string BranchName { get; private set; }
        public DateTime Created { get; private set; }
        public string PackagePath { get; private set; }

        public string SourceAuthor { get; private set; }

        public IEnumerable<BuildStep> Steps
        {
            get { return _buildSteps; }
        }

        public bool IsSuccessful
        {
            get { return Steps.All(s => s.IsSuccessful); }
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

        public void AssignId(int id)
        {
            PipelineId = id;
        }
    }
}