namespace Uncas.BuildPipeline.Web.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Uncas.BuildPipeline.Web.Models;
    using Uncas.BuildPipeline.Web.ViewModels;

    public static class PipelineMapper
    {
        public static PipelineIndexViewModel MapToPipelineIndexViewModel(IEnumerable<Pipeline> pipelines)
        {
            var result = new PipelineIndexViewModel();
            result.Pipelines = MapToPipelineViewModels(pipelines);
            PopulateBaseViewModel(result);
            return result;
        }

        private static IEnumerable<PipelineViewModel>
           MapToPipelineViewModels(IEnumerable<Pipeline> pipelines)
        {
            var viewModels = pipelines.Select(
                p => MapToPipelineViewModel(p));
            return viewModels;
        }

        private static void PopulateBaseViewModel(BaseViewModel baseViewModel)
        {
            //bool showDeployment = bool.Parse(ConfigurationManager.AppSettings["showDeployment"]);
            bool showDeployment = HttpContext.Current.Request.Url.AbsoluteUri.Contains("51743");
            baseViewModel.ShowDeployment = showDeployment;
        }

        private static PipelineViewModel MapToPipelineViewModel(Pipeline pipeline)
        {
            string createdDisplay = GetDateTimeDisplay(pipeline.Created);
            string sourceUrlRelative = pipeline.SourceUrl.Replace(pipeline.SourceUrlBase, "");
            if (sourceUrlRelative.Contains("/"))
                sourceUrlRelative = sourceUrlRelative.Split('/').Last();
            var result = new PipelineViewModel
            {
                ProjectName = pipeline.ProjectName,
                SourceAuthor = pipeline.SourceAuthor,
                SourceRevision = pipeline.SourceRevision,
                SourceUrlRelative = sourceUrlRelative,
                CreatedDisplay = createdDisplay,
                StatusText = pipeline.IsSuccessful ? "OK" : "Failed",
                CssClass = pipeline.IsSuccessful ? "BuildGreen" : "BuildRed"
            };
            result.Steps = pipeline.Steps.Select(MapToBuildStepViewModel);
            PopulateBaseViewModel(result);
            return result;
        }

        private static string GetDateTimeDisplay(DateTime dateTime)
        {
            TimeSpan timeSince = DateTime.Now.Subtract(dateTime);
            string dateTimeLabel = string.Empty;
            int dateTimeDisplayNumber = 0;
            if (timeSince.TotalDays >= 1d)
            {
                dateTimeLabel = "day";
                dateTimeDisplayNumber = (int)timeSince.TotalDays;
            }
            else if (timeSince.TotalHours >= 1d)
            {
                dateTimeLabel = "hour";
                dateTimeDisplayNumber = (int)timeSince.TotalHours;
            }
            else
            {
                dateTimeLabel = "minute";
                dateTimeDisplayNumber = (int)timeSince.TotalMinutes;
            }

            string pluralis = dateTimeDisplayNumber == 1 ?
                string.Empty : "s";
            return string.Format(
                "{0} {1}{2} ago",
                dateTimeDisplayNumber,
                dateTimeLabel,
                pluralis);
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