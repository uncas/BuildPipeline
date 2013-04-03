using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Commands
{
    public class UpdateCommitsHandler : ICommandHandler<UpdateCommits>
    {
        private readonly ICommitRepository _commitRepository;
        private readonly IGitUtility _gitUtility;
        private readonly ILogger _logger;
        private readonly IProjectReadStore _projectReadStore;

        public UpdateCommitsHandler(
            ICommitRepository commitRepository,
            IProjectReadStore projectReadStore,
            IGitUtility gitUtility,
            ILogger logger)
        {
            _commitRepository = commitRepository;
            _projectReadStore = projectReadStore;
            _gitUtility = gitUtility;
            _logger = logger;
        }

        #region ICommandHandler<UpdateCommits> Members

        public void Handle(UpdateCommits command)
        {
            IEnumerable<ProjectReadModel> projectsWithGitRemote =
                _projectReadStore.GetProjects().Where(
                    x => !string.IsNullOrWhiteSpace(x.GitRemoteUrl));
            IEnumerable<CommitReadModel> revisionsWithoutCommits =
                _commitRepository.GetRevisionsWithoutCommits(projectsWithGitRemote.Select(
                    x => x.ProjectId));
            _logger.Info("Found {0} revisions without commits.",
                         revisionsWithoutCommits.Count());
            foreach (CommitReadModel commitReadModel in revisionsWithoutCommits)
                GetAndAddCommit(projectsWithGitRemote, commitReadModel.ProjectId,
                                commitReadModel.Revision);
        }

        #endregion

        private void GetAndAddCommit(
            IEnumerable<ProjectReadModel> projects,
            int projectId,
            string revision)
        {
            ProjectReadModel project =
                projects.SingleOrDefault(
                    x => x.ProjectId == projectId);
            if (project == null)
                return;
            string localMirror = Path.Combine(UpdateGitMirrorsHandler.MirrorsFolder,
                                              project.ProjectName);
            GitLog gitLog = _gitUtility.GetLogs(localMirror,
                                                string.Format("{0}~",
                                                              revision),
                                                revision, null, 1).SingleOrDefault();
            if (gitLog == null)
                return;

            _logger.Debug("Found commit for project '{0}' and revision '{1}'.",
                          project.ProjectName, revision);
            _commitRepository.Add(new CommitReadModel
                {
                    ProjectId = projectId,
                    Revision = gitLog.Revision,
                    Subject = gitLog.Subject,
                    AuthorName = gitLog.AuthorName,
                    AuthorEmail = gitLog.AuthorEmail,
                    AuthorDate = gitLog.AuthorDate
                });
        }
    }
}