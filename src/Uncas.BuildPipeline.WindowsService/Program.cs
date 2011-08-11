namespace Uncas.BuildPipeline.WindowsService
{
    using System;
    using System.ServiceProcess;
    using Uncas.BuildPipeline.ApplicationServices;
    using Uncas.Core.Services;

    internal static class Program
    {
        /// <summary>
        /// Name of the service as defined in the Service component.
        /// </summary>
        private const string ServiceName = "DeployService";

        private static readonly Func<ServiceBase> GetServiceToRun =
            () => new DeployService();

        private static readonly Action ActionToRun =
            () => Bootstrapper.Resolve<IDeploymentService>().DeployDueDeployments();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            var programRunner = new ProgramRunner(
                ActionToRun,
                ServiceName,
                GetServiceToRun);
            programRunner.RunProgram(args);
        }
    }
}