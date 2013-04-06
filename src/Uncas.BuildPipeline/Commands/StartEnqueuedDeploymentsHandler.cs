using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Utilities;
using Uncas.Core.Data;
using Environment = Uncas.BuildPipeline.Models.Environment;

namespace Uncas.BuildPipeline.Commands
{
    public class StartEnqueuedDeploymentsHandler :
        ICommandHandler<StartEnqueuedDeployments>
    {
        private readonly IDeploymentRepository _deploymentRepository;
        private readonly IDeploymentUtility _deploymentUtility;
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly ILogger _logger;
        private readonly IPipelineRepository _pipelineRepository;
        private readonly IProjectReadStore _projectReadStore;

        public StartEnqueuedDeploymentsHandler(
            IEnvironmentRepository environmentRepository,
            IPipelineRepository pipelineRepository,
            IDeploymentRepository deploymentRepository,
            IDeploymentUtility deploymentUtility,
            IProjectReadStore projectReadStore,
            ILogger logger)
        {
            _environmentRepository = environmentRepository;
            _pipelineRepository = pipelineRepository;
            _deploymentRepository = deploymentRepository;
            _deploymentUtility = deploymentUtility;
            _projectReadStore = projectReadStore;
            _logger = logger;
        }

        #region ICommandHandler<StartEnqueuedDeployments> Members

        public void Handle(StartEnqueuedDeployments command)
        {
            const int pageSize = 30;
            IEnumerable<Deployment> dueDeployments =
                _deploymentRepository.GetDueDeployments(new PagingInfo(pageSize));
            _logger.Info("Starting '{0}' deployments.", dueDeployments.Count());
            foreach (Deployment deployment in dueDeployments)
            {
                _logger.Debug(
                    "Starting deployment of pipeline ID '{0}' to environment ID '{1}'.",
                    deployment.PipelineId,
                    deployment.EnvironmentId);
                deployment.MarkAsStarted();
                _deploymentRepository.UpdateDeployment(deployment);
                Deploy(
                    deployment.PipelineId,
                    deployment.EnvironmentId);
                deployment.MarkAsCompleted();
                _deploymentRepository.UpdateDeployment(deployment);
                _logger.Debug(
                    "Completed deployment of pipeline ID '{0}' to environment ID '{1}'.",
                    deployment.PipelineId,
                    deployment.EnvironmentId);
            }
        }

        #endregion

        private void Deploy(
            int pipelineId,
            int environmentId)
        {
            Pipeline pipeline =
                _pipelineRepository.GetPipeline(pipelineId);
            if (pipeline == null)
                throw new ArgumentException(
                    "The id does not correspond to an existing pipeline.",
                    "pipelineId");

            Environment environment =
                _environmentRepository.GetEnvironment(environmentId);
            if (environment == null)
                throw new ArgumentException(
                    "The id does not correspond to an existing environment.",
                    "environmentId");

            ProjectReadModel project = _projectReadStore.GetProjectById(pipeline.ProjectId);
            if (project == null)
                throw new InvalidOperationException("Cannot deploy without a project.");
            string packagePath = Path.Combine(DeploymentUtility.PackageFolder,
                                              pipeline.PackagePath);

            _deploymentUtility.Deploy(
                packagePath,
                environment,
                project);
        }
    }
}