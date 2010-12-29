namespace Uncas.BuildPipeline.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Pipeline
    {
        private IList<BuildStep> steps;

        public Pipeline()
        {
            this.steps = new List<BuildStep>();
        }

        public int Id { get; set; }
        public string ProjectName { get; set; }
        public int SourceRevision { get; set; }
        public string SourceUrl { get; set; }
        public string SourceUrlBase { get; set; }
        public DateTime Created { get; set; }

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