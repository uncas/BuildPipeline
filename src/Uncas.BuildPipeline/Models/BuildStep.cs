namespace Uncas.BuildPipeline.Models
{
    using System;

    public class BuildStep
    {
        public BuildStep(
            bool isSuccessful,
            string stepName,
            DateTime created)
        {
            this.IsSuccessful = isSuccessful;
            this.StepName = stepName;
            this.Created = created;
        }

        public DateTime Created { get; private set; }
        public bool IsSuccessful { get; private set; }
        public string StepName { get; private set; }
    }
}