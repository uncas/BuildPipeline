namespace Uncas.BuildPipeline.Web.Models
{
    using System.Collections.Generic;

    public class Build
    {
        public string ProjectName { get; set; }
        public int SourceRevision { get; set; }
        public IEnumerable<BuildStep> Steps { get; set; }
    }

    public class BuildStep
    {
        public bool IsSuccessful { get; set; }
        public string StepName { get; set; }
    }
}