using System;
using System.Collections.Generic;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Utilities;
using Uncas.Core.Data;
using Environment = Uncas.BuildPipeline.Models.Environment;

namespace Uncas.BuildPipeline.Commands
{
    public class StartEnqueuedDeploymentsHandler : ICommandHandler<StartEnqueuedDeployments>
    {
        private const string WorkingDirectory = @"C:\temp\DeploymentWork";

        private readonly IDeploymentRepository _deploymentRepository;
        private readonly IDeploymentUtility _deploymentUtility;
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly IPipelineRepository _pipelineRepository;

        public StartEnqueuedDeploymentsHandler(IEnvironmentRepository environmentRepository,
                                               IPipelineRepository pipelineRepository,
                                               IDeploymentRepository deploymentRepository,
                                               IDeploymentUtility deploymentUtility)
        {
            _environmentRepository = environmentRepository;
            _pipelineRepository = pipelineRepository;
            _deploymentRepository = deploymentRepository;
            _deploymentUtility = deploymentUtility;
        }

        #region ICommandHandler<StartEnqueuedDeployments> Members

        public void Handle(StartEnqueuedDeployments command)
        {
            const int pageSize = 30;
            IEnumerable<Deployment> dueDeployments =
                _deploymentRepository.GetDueDeployments(new PagingInfo(pageSize));
            foreach (Deployment deployment in dueDeployments)
            {
                deployment.MarkAsStarted();
                _deploymentRepository.UpdateDeployment(deployment);
                Deploy(
                    deployment.PipelineId,
                    deployment.EnvironmentId);
                deployment.MarkAsCompleted();
                _deploymentRepository.UpdateDeployment(deployment);
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

            string packagePath = pipeline.PackagePath;
            _deploymentUtility.Deploy(
                packagePath,
                WorkingDirectory,
                environment);
        }
    }
}