namespace Uncas.BuildPipeline.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Pipeline
    {
        private IList<BuildStep> steps;

        public Pipeline(
            int id,
            string projectName,
            int sourceRevision,
            string sourceUrl,
            string sourceUrlBase,
            DateTime created,
            string sourceAuthor,
            string packageUrl)
        {
            this.steps = new List<BuildStep>();
            this.Id = id;
            this.ProjectName = projectName;
            this.SourceRevision = sourceRevision;
            this.SourceUrl = sourceUrl;
            this.SourceUrlBase = sourceUrlBase;
            this.Created = created;
            this.SourceAuthor = sourceAuthor;
            this.PackageUrl = packageUrl;
        }

        public int Id { get; private set; }
        public string ProjectName { get; private set; }
        public string SourceAuthor { get; private set; }
        public int SourceRevision { get; private set; }
        public string SourceUrl { get; private set; }
        public string SourceUrlBase { get; private set; }
        public DateTime Created { get; private set; }
        public string PackageUrl { get; private set; }

        public IEnumerable<BuildStep> Steps
        {
            get
            {
                return this.steps;
            }
        }

        public bool IsSuccessful
        {
            get
            {
                return !Steps.Any(s => !s.IsSuccessful);
            }
        }

        public void AddStep(BuildStep buildStep)
        {
            // Removing any existing steps with the same name:
            var existing = this.steps.FirstOrDefault(
                s => s.StepName == buildStep.StepName);
            if (existing != null)
                this.steps.Remove(existing);

            // Adding the new step:
            this.steps.Add(buildStep);
        }
    }
}