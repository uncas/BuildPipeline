using System;
using System.ServiceProcess;
using Uncas.BuildPipeline.ApplicationServices;
using Uncas.BuildPipeline.Commands;
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

        private static void TheAction()
        {
            try
            {
                Bootstrapper.Resolve<IDeploymentService>().DeployDueDeployments();
                Bootstrapper.Resolve<ICommandBus>().Publish(new UpdateGitMirrors());
            }
            catch (Exception e)
            {
                var logger = Bootstrapper.Resolve<ILogger>();
                logger.Error(e, "Error when running actions.");
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            var logger = Bootstrapper.Resolve<ILogger>();
            logger.Info("Starting Windows service.");
            var programRunner = new ProgramRunner(
                TheAction,
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