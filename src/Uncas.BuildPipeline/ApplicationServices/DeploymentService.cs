namespace Uncas.BuildPipeline.ApplicationServices
{
    using System;
    using System.Collections.Generic;
    using Uncas.BuildPipeline.ApplicationServices.Results;
    using Uncas.BuildPipeline.Models;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.BuildPipeline.Utilities;
    using Environment = Uncas.BuildPipeline.Models.Environment;

    public class DeploymentService : IDeploymentService
    {
        private const string WorkingDirectory = @"C:\temp\DeploymentWork";

        private readonly IDeploymentRepository deploymentRepository;
        private readonly IDeploymentUtility deploymentUtility;
        private readonly IEnvironmentRepository environmentRepository;
        private readonly IPipelineRepository pipelineRepository;

        public DeploymentService(
            IEnvironmentRepository environmentRepository,
            IPipelineRepository pipelineRepository,
            IDeploymentRepository deploymentRepository,
            IDeploymentUtility deploymentUtility)
        {
            this.deploymentRepository = deploymentRepository;
            this.environmentRepository = environmentRepository;
            this.pipelineRepository = pipelineRepository;
            this.deploymentUtility = deploymentUtility;
        }

        public void Deploy(
            int pipelineId,
            int environmentId)
        {
            Pipeline pipeline =
                pipelineRepository.GetPipeline(pipelineId);
            if (pipeline == null)
            {
                throw new ArgumentException(
                    "The id does not correspond to an existing pipeline.",
                    "pipelineId");
            }

            Environment environment =
                environmentRepository.GetEnvironment(environmentId);
            if (environment == null)
            {
                throw new ArgumentException(
                    "The id does not correspond to an existing environment.",
                    "environmentId");
            }

            string packagePath = pipeline.PackagePath;
            deploymentUtility.Deploy(
                packagePath,
                WorkingDirectory,
                environment);
        }

        public void DeployDueDeployments()
        {
            IEnumerable<Deployment> dueDeployments =
                deploymentRepository.GetDueDeployments();
            foreach (Deployment deployment in dueDeployments)
            {
                deployment.Start();
                deploymentRepository.UpdateDeployment(deployment);
                Deploy(
                    deployment.PipelineId,
                    deployment.EnvironmentId);
                deployment.Complete();
                deploymentRepository.UpdateDeployment(deployment);
            }
        }

        public IEnumerable<Deployment> GetDeployments(int pipelineId)
        {
            return deploymentRepository.GetDeployments(
                pipelineId);
        }

        public IEnumerable<Deployment> GetDeploymentsByEnvironment(
            int environmentId)
        {
            return deploymentRepository.GetByEnvironment(
                environmentId);
        }

        public IEnumerable<Deployment> GetDueDeployments()
        {
            return deploymentRepository.GetDueDeployments();
        }

        public ScheduleDeploymentResult ScheduleDeployment(
            int pipelineId,
            int environmentId)
        {
            var result = new ScheduleDeploymentResult();

            Pipeline pipeline =
                pipelineRepository.GetPipeline(pipelineId);
            if (pipeline == null)
            {
                result.AddError(new ResultError("The id does not correspond to an existing pipeline."));
            }

            Environment environment =
                environmentRepository.GetEnvironment(environmentId);
            if (environment == null)
            {
                result.AddError(new ResultError("The id does not correspond to an existing environment."));
            }

            if (result.HasErrors)
            {
                return result;
            }

            var deployment = new Deployment(
                pipelineId,
                environmentId);
            deploymentRepository.AddDeployment(deployment);
            return new ScheduleDeploymentResult(deployment);
        }
    }
}