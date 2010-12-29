namespace Uncas.BuildPipeline.Web.Models
{
    using System;

    public class BuildStep
    {
        public DateTime Created { get; set; }
        public bool IsSuccessful { get; set; }
        public string StepName { get; set; }
    }
}