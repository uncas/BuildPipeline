using System;
using System.Configuration;
using System.IO;
using Environment = Uncas.BuildPipeline.Models.Environment;

namespace Uncas.BuildPipeline.Utilities
{
    public class DeploymentUtility : IDeploymentUtility
    {
        private const string WorkingDirectory = @"C:\temp\DeploymentWork";

        public static readonly string PackageFolder =
            ConfigurationManager.AppSettings["DeploymentPackageFolder"] ??
            @"C:\Temp\DeploymentPackages";

        private readonly IFileUtility _fileUtility;
        private readonly IPowershellUtility _powershellUtility;
        private readonly IZipUtility _zipUtility;

        public DeploymentUtility(
            IZipUtility zipUtility,
            IPowershellUtility powershellUtility,
            IFileUtility fileUtility)
        {
            _zipUtility = zipUtility;
            _powershellUtility = powershellUtility;
            _fileUtility = fileUtility;
        }

        #region IDeploymentUtility Members

        public void Deploy(
            string packagePath,
            Environment environment,
            string customScript)
        {
            _zipUtility.ExtractZipFile(packagePath, WorkingDirectory);
            string scriptContents = string.Format(@"
param ($environmentName)
{0}", customScript);
            string scriptTempPath
                = Path.Combine(WorkingDirectory, Guid.NewGuid().ToString() + ".ps1");
            _fileUtility.WriteAllText(scriptTempPath, scriptContents);
            string arguments =
                string.Format(@"-NonInteractive -File {0} -environmentName ""{1}""",
                              scriptTempPath,
                              environment.EnvironmentName);

            // TODO: Collect errors from powershell execution...
            _powershellUtility.RunPowershell(WorkingDirectory, arguments);
        }

        #endregion
    }
}