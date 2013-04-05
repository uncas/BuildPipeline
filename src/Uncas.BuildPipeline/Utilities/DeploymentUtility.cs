using System;
using System.Configuration;
using System.IO;
using Environment = Uncas.BuildPipeline.Models.Environment;

namespace Uncas.BuildPipeline.Utilities
{
    public class DeploymentUtility : IDeploymentUtility
    {
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
            string workingDirectory,
            Environment environment,
            string customScript)
        {
            _zipUtility.ExtractZipFile(packagePath, workingDirectory);
            string scriptContents = string.Format(@"
param ($environmentName)
{0}", customScript);
            string scriptTempPath
                = Path.Combine(workingDirectory, Guid.NewGuid().ToString() + ".ps1");
            _fileUtility.WriteAllText(scriptTempPath, scriptContents);
            string arguments =
                string.Format(@"-NonInteractive -File {0} -environmentName ""{1}""",
                              scriptTempPath,
                              environment.EnvironmentName);

            // TODO: Collect errors from powershell execution...
            _powershellUtility.RunPowershell(workingDirectory, arguments);
        }

        #endregion
    }
}