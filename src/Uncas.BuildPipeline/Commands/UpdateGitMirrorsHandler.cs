using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Commands
{
    public class UpdateGitMirrorsHandler : ICommandHandler<UpdateGitMirrors>
    {
        private readonly IGitUtility _gitUtility;

        public UpdateGitMirrorsHandler(IGitUtility gitUtility)
        {
            _gitUtility = gitUtility;
        }

        #region ICommandHandler<UpdateGitMirrors> Members

        public void Handle(UpdateGitMirrors command)
        {
            const string mirrorsFolder = @"C:\Temp\Mirrors";
            const string remoteUrl = "git://github.com/uncas/BuildPipeline.git";
            _gitUtility.Mirror(remoteUrl, mirrorsFolder, "BuildPipeline");
        }

        #endregion
    }
}