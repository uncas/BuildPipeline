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
                SourceRevision = build.SourceRevision,
                SourceUrlRelative = build.SourceUrl.Replace(build.SourceUrlBase, "")
            };
            // TODO: Refactor: Not satisfactory:
            BuildStep stepCommit = build.Steps.ElementAt(0);
            result.StepCommit = MapToBuildStepViewModel(stepCommit);
            if (build.Steps.Count() > 1)
            {
                BuildStep stepAcceptance = build.Steps.ElementAt(1);
                result.StepAcceptance =
                    MapToBuildStepViewModel(stepAcceptance);
            }
            return result;
        }

        private static BuildStepViewModel
            MapToBuildStepViewModel(BuildStep step)
        {
            return new BuildStepViewModel
            {
                StatusText = step.IsSuccessful ? "OK" : "Failed",
                StepName = step.StepName,
                CssClass = step.IsSuccessful ? "BuildStep BuildGreen" : "BuildStep BuildRed"
            };
        }
    }
}