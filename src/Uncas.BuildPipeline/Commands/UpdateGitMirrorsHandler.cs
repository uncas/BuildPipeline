using System.Collections.Generic;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Commands
{
    public class UpdateGitMirrorsHandler : ICommandHandler<UpdateGitMirrors>
    {
        private readonly IGitUtility _gitUtility;
        private readonly IProjectReadStore _projectReadStore;

        public UpdateGitMirrorsHandler(
            IGitUtility gitUtility,
            IProjectReadStore projectReadStore)
        {
            _gitUtility = gitUtility;
            _projectReadStore = projectReadStore;
        }

        #region ICommandHandler<UpdateGitMirrors> Members

        public void Handle(UpdateGitMirrors command)
        {
            const string mirrorsFolder = @"C:\Temp\Mirrors";
            IEnumerable<ProjectReadModel> projects = _projectReadStore.GetProjects();
            foreach (ProjectReadModel project in projects)
            {
                string remoteUrl = project.GitRemoteUrl;
                if (string.IsNullOrWhiteSpace(remoteUrl))
                    continue;
                _gitUtility.Mirror(remoteUrl, mirrorsFolder, project.ProjectName);
            }
        }

        #endregion
    }
}