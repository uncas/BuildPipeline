using System;
using System.Configuration;
using System.IO;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Utilities;
using Environment = Uncas.BuildPipeline.Models.Environment;

namespace Uncas.BuildPipeline.DomainServices
{
    public class PowershellDeployment : IDeploymentMethod
    {
        private const string WorkingDirectory = @"C:\temp\DeploymentWork";

        public static readonly string PackageFolder =
            ConfigurationManager.AppSettings["DeploymentPackageFolder"] ??
            @"C:\Temp\DeploymentPackages";

        private readonly IFileUtility _fileUtility;
        private readonly ILogger _logger;
        private readonly IPowershellUtility _powershellUtility;
        private readonly IZipUtility _zipUtility;

        public PowershellDeployment(
            IZipUtility zipUtility,
            IPowershellUtility powershellUtility,
            IFileUtility fileUtility,
            ILogger logger)
        {
            _zipUtility = zipUtility;
            _powershellUtility = powershellUtility;
            _fileUtility = fileUtility;
            _logger = logger;
        }

        #region IDeploymentMethod Members

        public void Deploy(
            string packagePath,
            Environment environment,
            ProjectReadModel project)
        {
            string customScript = project.DeploymentScript;
            if (string.IsNullOrWhiteSpace(customScript))
            {
                _logger.Debug("No deployment script for project '{0}'.",
                              project.ProjectName);
                return;
            }

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