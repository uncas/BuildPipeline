namespace Uncas.BuildPipeline.ApplicationServices
{
    using Uncas.BuildPipeline.Models;
    using Uncas.BuildPipeline.Repositories;
    using Uncas.BuildPipeline.Utilities;

    public class DeploymentService
    {
        private readonly EnvironmentRepository environmentRepository;
        private readonly PipelineRepository pipelineRepository;
        private readonly DeploymentUtility deploymentUtility;

        public DeploymentService(
            EnvironmentRepository environmentRepository,
            PipelineRepository pipelineRepository,
            DeploymentUtility deploymentUtility)
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
            Environment environment =
                this.environmentRepository.GetEnvironment(environmentId);
            string packagePath = pipeline.PackagePath;
            string workingDirectory = @"C:\temp\deploytest";
            this.deploymentUtility.Deploy(
                packagePath,
                workingDirectory,
                environment);
        }
    }
}