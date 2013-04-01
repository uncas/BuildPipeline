using System;
using System.Collections.Generic;
using System.Linq;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Utilities;
using Uncas.BuildPipeline.Web.Mappers;

namespace Uncas.BuildPipeline.Web.ViewModels
{
    public class PipelineListItemViewModel
    {
        public int PipelineId { get; set; }
        public string BranchName { get; set; }
        public string Revision { get; set; }
        public string PackagePath { get; set; }
        public DateTime Created { get; set; }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string GithubUrl { get; set; }

        public string SourceAuthor { get; set; }

        public string CommitLink
        {
            get
            {
                if (string.IsNullOrWhiteSpace(GithubUrl))
                    return string.Empty;
                return GitHubLink.Revision(GithubUrl, Revision);
            }
        }

        public IEnumerable<BuildStepViewModel> Steps { get; private set; }

        public string StatusText { get; private set; }
        public string CssClass { get; private set; }

        public string CreatedDisplay
        {
            get { return PipelineMapper.GetDateTimeDisplay(Created); }
        }

        public void AddSteps(IEnumerable<BuildStep> query)
        {
            Steps = query.Select(PipelineMapper.MapToBuildStepViewModel);
            bool isSuccessful = query.All(x => x.IsSuccessful);
            CssClass = isSuccessful ? "PipelineGreen" : "PipelineRed";
            StatusText = isSuccessful ? "OK" : "Failed";
        }
    }
}