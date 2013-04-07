using System.Collections.Generic;
using System.Linq;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Commands
{
    public class UpdateGitMirrorsHandler : ICommandHandler<UpdateGitMirrors>
    {
        public const string MirrorsFolder = @"C:\Temp\Mirrors";

        private readonly IGitUtility _gitUtility;
        private readonly ILogger _logger;
        private readonly IProjectReadStore _projectReadStore;

        public UpdateGitMirrorsHandler(
            IGitUtility gitUtility,
            IProjectReadStore projectReadStore,
            ILogger logger)
        {
            _gitUtility = gitUtility;
            _projectReadStore = projectReadStore;
            _logger = logger;
        }

        #region ICommandHandler<UpdateGitMirrors> Members

        public void Handle(UpdateGitMirrors command)
        {
            _logger.Debug("Updating all mirrors.");
            IEnumerable<ProjectReadModel> projects = _projectReadStore.GetProjects();
            _logger.Debug("Updating mirrors for '{0}' projects.", projects.Count());
            foreach (ProjectReadModel project in projects)
            {
                string remoteUrl = project.GitRemoteUrl;
                if (string.IsNullOrWhiteSpace(remoteUrl))
                    continue;
                _logger.Debug("Updating mirror for project '{0}'.", project.ProjectName);
                _gitUtility.Mirror(remoteUrl, MirrorsFolder, project.ProjectName);
                _logger.Debug("Updated mirror for project '{0}'.", project.ProjectName);
            }
        }

        #endregion
    }
}