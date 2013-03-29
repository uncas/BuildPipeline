using System;
using System.ServiceProcess;
using Uncas.BuildPipeline.ApplicationServices;
using Uncas.Core.Services;

namespace Uncas.BuildPipeline.WindowsService
{
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
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            var logger = Bootstrapper.Resolve<ILogger>();
            logger.Info("Starting Windows service.");
            var programRunner = new ProgramRunner(
                ActionToRun,
                ServiceName,
                GetServiceToRun);

            try
            {
                programRunner.RunProgram(args);
            }
            catch (Exception e)
            {
                logger.Error(e, "Unhandled exception in Windows service.");
            }

            logger.Info("Stopping Windows service.");
        }
    }
}