namespace Uncas.BuildPipeline.ApplicationServices
{
    using System;
    using System.Collections.Generic;
    using Uncas.BuildPipeline.ApplicationServices.Results;
    using Uncas.BuildPipeline.Models;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.BuildPipeline.Utilities;

    public class DeploymentService : IDeploymentService
    {
        private const string WorkingDirectory = @"C:\temp\DeploymentWork";

        private readonly IEnvironmentRepository environmentRepository;
        private readonly IPipelineRepository pipelineRepository;
        private readonly IDeploymentRepository deploymentRepository;
        private readonly IDeploymentUtility deploymentUtility;

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
            var pipeline =
                this.pipelineRepository.GetPipeline(pipelineId);
            if (pipeline == null)
            {
                throw new ArgumentException(
                    "The id does not correspond to an existing pipeline.",
                    "pipelineId");
            }

            Models.Environment environment =
                this.environmentRepository.GetEnvironment(environmentId);
            if (environment == null)
            {
                throw new ArgumentException(
                    "The id does not correspond to an existing environment.",
                    "environmentId");
            }

            string packagePath = pipeline.PackagePath;
            this.deploymentUtility.Deploy(
                packagePath,
                WorkingDirectory,
                environment);
        }

        public void DeployDueDeployments()
        {
            var dueDeployments =
                this.deploymentRepository.GetDueDeployments();
            foreach (var deployment in dueDeployments)
            {
                deployment.Start();
                this.deploymentRepository.UpdateDeployment(deployment);
                this.Deploy(
                    deployment.PipelineId,
                    deployment.EnvironmentId);
                deployment.Complete();
                this.deploymentRepository.UpdateDeployment(deployment);
            }
        }

        public ScheduleDeploymentResult ScheduleDeployment(
            int pipelineId,
            int environmentId)
        {
            var result = new ScheduleDeploymentResult();

            var pipeline =
                this.pipelineRepository.GetPipeline(pipelineId);
            if (pipeline == null)
            {
                result.AddError(new ResultError("The id does not correspond to an existing pipeline."));
            }

            Models.Environment environment =
                this.environmentRepository.GetEnvironment(environmentId);
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
            this.deploymentRepository.AddDeployment(deployment);
            return new ScheduleDeploymentResult(deployment);
        }

        public IEnumerable<Deployment> GetDueDeployments()
        {
            return this.deploymentRepository.GetDueDeployments();
        }

        public IEnumerable<Deployment> GetDeployments(int pipelineId)
        {
            return this.deploymentRepository.GetDeployments(
                pipelineId);
        }

        public IEnumerable<Deployment> GetDeploymentsByEnvironment(
            int environmentId)
        {
            return this.deploymentRepository.GetByEnvironment(
                environmentId);
        }
    }
}