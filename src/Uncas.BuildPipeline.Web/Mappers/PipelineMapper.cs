namespace Uncas.BuildPipeline.Web.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Uncas.BuildPipeline.Web.Models;
    using Uncas.BuildPipeline.Web.ViewModels;

    public static class PipelineMapper
    {
        public static IEnumerable<PipelineViewModel>
           MapToPipelineViewModels(IEnumerable<Pipeline> pipelines)
        {
            var viewModels = pipelines.Select(
                p => MapToPipelineViewModel(p));
            return viewModels;
        }

        private static PipelineViewModel MapToPipelineViewModel(Pipeline pipeline)
        {
            string createdDisplay = GetDateTimeDisplay(pipeline.Created);

            var result = new PipelineViewModel
            {
                ProjectName = pipeline.ProjectName,
                SourceRevision = pipeline.SourceRevision,
                SourceUrlRelative = pipeline.SourceUrl.Replace(pipeline.SourceUrlBase, ""),
                CreatedDisplay = createdDisplay,
                StatusText = pipeline.IsSuccessful ? "OK" : "Failed",
                CssClass = pipeline.IsSuccessful ? "BuildGreen" : "BuildRed"
            };
            result.Steps = pipeline.Steps.Select(MapToBuildStepViewModel);
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