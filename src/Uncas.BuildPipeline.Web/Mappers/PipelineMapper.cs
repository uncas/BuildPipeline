namespace Uncas.BuildPipeline.Web.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Uncas.BuildPipeline.Models;
    using Uncas.BuildPipeline.Web.ViewModels;
    using System.Globalization;

    public static class PipelineMapper
    {
        public static PipelineIndexViewModel MapToPipelineIndexViewModel(IEnumerable<Pipeline> pipelines)
        {
            var result = new PipelineIndexViewModel();
            result.Pipelines = MapToPipelineViewModels(pipelines);
            return result;
        }

        public static PipelineViewModel MapToPipelineViewModel(Pipeline pipeline)
        {
            if (pipeline == null)
            {
                throw new ArgumentNullException("pipeline");
            }

            string createdDisplay = GetDateTimeDisplay(pipeline.Created);
            string sourceUrlRelative = pipeline.SourceUrl.Replace(pipeline.SourceUrlBase, string.Empty);
            if (sourceUrlRelative.Contains("/"))
                sourceUrlRelative = sourceUrlRelative.Split('/').Last();
            var result = new PipelineViewModel
            {
                PipelineId = pipeline.Id,
                ProjectName = pipeline.ProjectName,
                SourceAuthor = pipeline.SourceAuthor,
                SourceRevision = pipeline.SourceRevision,
                SourceUrlRelative = sourceUrlRelative,
                CreatedDisplay = createdDisplay,
                StatusText = pipeline.IsSuccessful ? "OK" : "Failed",
                CssClass = pipeline.IsSuccessful ? "PipelineGreen" : "PipelineRed"
            };
            result.Steps = pipeline.Steps.Select(MapToBuildStepViewModel);
            return result;
        }

        private static IEnumerable<PipelineViewModel>
           MapToPipelineViewModels(IEnumerable<Pipeline> pipelines)
        {
            var viewModels = pipelines.Select(
                p => MapToPipelineViewModel(p));
            return viewModels;
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
                CultureInfo.CurrentCulture,
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
                CssClass = step.IsSuccessful ? "BuildGreen" : "BuildRed"
            };
        }
    }
}