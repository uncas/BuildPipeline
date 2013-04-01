using System;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Repositories;
using Environment = Uncas.BuildPipeline.Models.Environment;

namespace Uncas.BuildPipeline.Commands
{
    public class EnqueueDeploymentHandler : ICommandHandler<EnqueueDeployment>
    {
        private readonly IDeploymentRepository _deploymentRepository;
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly IPipelineRepository _pipelineRepository;

        public EnqueueDeploymentHandler(IPipelineRepository pipelineRepository,
                                        IEnvironmentRepository environmentRepository,
                                        IDeploymentRepository deploymentRepository)
        {
            _pipelineRepository = pipelineRepository;
            _environmentRepository = environmentRepository;
            _deploymentRepository = deploymentRepository;
        }

        #region ICommandHandler<EnqueueDeployment> Members

        public void Handle(EnqueueDeployment command)
        {
            int pipelineId = command.PipelineId;
            int environmentId = command.EnvironmentId;
            Pipeline pipeline =
                _pipelineRepository.GetPipeline(pipelineId);
            if (pipeline == null)
                throw new ArgumentException(
                    string.Format("Cannot enqueue a deployment without a pipeline. No pipeline for id '{0}'.",
                                  pipelineId));

            Environment environment =
                _environmentRepository.GetEnvironment(environmentId);
            if (environment == null)
                throw new ArgumentException(
                    string.Format("Cannot enqueue a deployment without an environment. No environment for id '{0}'.",
                                  environmentId));

            var deployment = new Deployment(
                pipelineId,
                environmentId);
            _deploymentRepository.AddDeployment(deployment);
        }

        #endregion
    }
}