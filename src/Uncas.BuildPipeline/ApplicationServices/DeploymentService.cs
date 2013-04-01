using System;
using System.Collections.Generic;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Uncas.BuildPipeline.Utilities;
using Uncas.Core.Data;
using Environment = Uncas.BuildPipeline.Models.Environment;

namespace Uncas.BuildPipeline.ApplicationServices
{
    public class DeploymentService : IDeploymentService
    {
        private const string WorkingDirectory = @"C:\temp\DeploymentWork";

        private readonly IDeploymentRepository _deploymentRepository;
        private readonly IDeploymentUtility _deploymentUtility;
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly IPipelineRepository _pipelineRepository;

        public DeploymentService(
            IEnvironmentRepository environmentRepository,
            IPipelineRepository pipelineRepository,
            IDeploymentRepository deploymentRepository,
            IDeploymentUtility deploymentUtility)
        {
            _deploymentRepository = deploymentRepository;
            _environmentRepository = environmentRepository;
            _pipelineRepository = pipelineRepository;
            _deploymentUtility = deploymentUtility;
        }

        #region IDeploymentService Members

        public void Deploy(
            int pipelineId,
            int environmentId)
        {
            Pipeline pipeline =
                _pipelineRepository.GetPipeline(pipelineId);
            if (pipeline == null)
            {
                throw new ArgumentException(
                    "The id does not correspond to an existing pipeline.",
                    "pipelineId");
            }

            Environment environment =
                _environmentRepository.GetEnvironment(environmentId);
            if (environment == null)
            {
                throw new ArgumentException(
                    "The id does not correspond to an existing environment.",
                    "environmentId");
            }

            string packagePath = pipeline.PackagePath;
            _deploymentUtility.Deploy(
                packagePath,
                WorkingDirectory,
                environment);
        }

        public void DeployDueDeployments()
        {
            const int pageSize = 30;
            IEnumerable<Deployment> dueDeployments =
                _deploymentRepository.GetDueDeployments(new PagingInfo(pageSize));
            foreach (Deployment deployment in dueDeployments)
            {
                deployment.Start();
                _deploymentRepository.UpdateDeployment(deployment);
                Deploy(
                    deployment.PipelineId,
                    deployment.EnvironmentId);
                deployment.Complete();
                _deploymentRepository.UpdateDeployment(deployment);
            }
        }

        public IEnumerable<Deployment> GetDeployments(int pipelineId)
        {
            return _deploymentRepository.GetDeployments(
                pipelineId);
        }

        public IEnumerable<Deployment> GetDeploymentsByEnvironment(
            int environmentId)
        {
            return _deploymentRepository.GetByEnvironment(
                environmentId);
        }

        public IEnumerable<Deployment> GetDueDeployments(PagingInfo pagingInfo)
        {
            return _deploymentRepository.GetDueDeployments(pagingInfo);
        }

        #endregion
    }
}