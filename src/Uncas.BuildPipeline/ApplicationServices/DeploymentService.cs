namespace Uncas.BuildPipeline.ApplicationServices
{
    using System;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.BuildPipeline.Utilities;

    public class DeploymentService : IDeploymentService
    {
        private readonly IEnvironmentRepository environmentRepository;
        private readonly IPipelineRepository pipelineRepository;
        private readonly IDeploymentUtility deploymentUtility;

        public DeploymentService(
            IEnvironmentRepository environmentRepository,
            IPipelineRepository pipelineRepository,
            IDeploymentUtility deploymentUtility)
        {
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
            string workingDirectory = @"C:\temp\deploytest";
            this.deploymentUtility.Deploy(
                packagePath,
                workingDirectory,
                environment);
        }
    }
}