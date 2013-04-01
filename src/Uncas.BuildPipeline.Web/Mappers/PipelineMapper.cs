using System;
using System.Globalization;
using System.Linq;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Web.ViewModels;

namespace Uncas.BuildPipeline.Web.Mappers
{
    /// <summary>
    /// Maps pipeline info.
    /// </summary>
    public static class PipelineMapper
    {
        public static PipelineViewModel MapToPipelineViewModel(Pipeline pipeline)
        {
            if (pipeline == null)
                throw new ArgumentNullException("pipeline");

            string createdDisplay = GetDateTimeDisplay(pipeline.Created);

            var result = new PipelineViewModel
                {
                    PipelineId = pipeline.PipelineId,
                    ProjectName = pipeline.ProjectName,
                    SourceAuthor = "Unknown",
                    Revision = pipeline.Revision,
                    BranchName = pipeline.BranchName,
                    CreatedDisplay = createdDisplay,
                    StatusText = pipeline.IsSuccessful ? "OK" : "Failed",
                    CssClass = pipeline.IsSuccessful ? "PipelineGreen" : "PipelineRed",
                    Steps = pipeline.Steps.Select(MapToBuildStepViewModel),
                    PackagePath = pipeline.PackagePath,
                    CommitLink = "UNDONE"
                };
            return result;
        }

        public static string GetDateTimeDisplay(DateTime dateTime)
        {
            TimeSpan timeSince = DateTime.Now.Subtract(dateTime);
            string dateTimeLabel;
            int dateTimeDisplayNumber;
            if (timeSince.TotalDays >= 1d)
            {
                dateTimeLabel = "day";
                dateTimeDisplayNumber = (int) timeSince.TotalDays;
            }
            else if (timeSince.TotalHours >= 1d)
            {
                dateTimeLabel = "hour";
                dateTimeDisplayNumber = (int) timeSince.TotalHours;
            }
            else
            {
                dateTimeLabel = "minute";
                dateTimeDisplayNumber = (int) timeSince.TotalMinutes;
            }

            string pluralis = dateTimeDisplayNumber == 1
                                  ? string.Empty
                                  : "s";
            return string.Format(
                CultureInfo.CurrentCulture,
                "{0} {1}{2} ago",
                dateTimeDisplayNumber,
                dateTimeLabel,
                pluralis);
        }

        public static BuildStepViewModel
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