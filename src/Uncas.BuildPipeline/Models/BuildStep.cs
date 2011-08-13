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
            IsSuccessful = isSuccessful;
            StepName = stepName;
            Created = created;
        }

        public DateTime Created { get; private set; }

        public bool IsSuccessful { get; private set; }

        public string StepName { get; private set; }
    }
}