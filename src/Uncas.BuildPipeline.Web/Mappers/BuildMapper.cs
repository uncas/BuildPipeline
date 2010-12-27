namespace Uncas.BuildPipeline.Web.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using Uncas.BuildPipeline.Web.Models;
    using Uncas.BuildPipeline.Web.ViewModels;

    public static class BuildMapper
    {
        public static IEnumerable<BuildViewModel>
           MapToBuildViewModels(IEnumerable<Build> builds)
        {
            var viewModels = builds.Select(
                b => MapToBuildViewModel(b));
            return viewModels;
        }

        private static BuildViewModel MapToBuildViewModel(Build build)
        {
            var result = new BuildViewModel
            {
                ProjectName = build.ProjectName,
                SourceRevision = build.SourceRevision
            };
            // TODO: Refactor: Not satisfactory:
            BuildStep stepUnit = build.Steps.ElementAt(0);
            result.StepUnit = MapToBuildStepViewModel(stepUnit);
            if (build.Steps.Count() > 1)
            {
                BuildStep stepIntegration = build.Steps.ElementAt(1);
                result.StepIntegration =
                    MapToBuildStepViewModel(stepIntegration);
            }
            return result;
        }

        private static BuildStepViewModel
            MapToBuildStepViewModel(BuildStep stepUnit)
        {
            return new BuildStepViewModel
            {
                StatusText = stepUnit.IsSuccessful ? "OK" : "Failed",
                StepName = stepUnit.StepName,
                CssClass = stepUnit.IsSuccessful ? "BuildStep BuildGreen" : "BuildStep BuildRed"
            };
        }
    }
}