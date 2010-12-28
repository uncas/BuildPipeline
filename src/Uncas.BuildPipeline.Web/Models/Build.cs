namespace Uncas.BuildPipeline.Web.Models
{
    using System;
    using System.Collections.Generic;

    public class Build
    {
        private IList<BuildStep> steps;

        public Build()
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

        public void AddStep(BuildStep buildStep)
        {
            this.steps.Add(buildStep);
        }
    }
}