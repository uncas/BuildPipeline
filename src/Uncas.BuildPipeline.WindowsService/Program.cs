using System;
using System.ServiceProcess;
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
            () => new DeployService(TheAction);

        private static void TheAction()
        {
            var logger = Bootstrapper.Resolve<ILogger>();
            logger.Debug("Starting action.");
            try
            {
                var commandBus = Bootstrapper.Resolve<ICommandBus>();
                commandBus.Publish(new UpdateGitMirrors());
                commandBus.Publish(new UpdateCommits());
                commandBus.Publish(new StartEnqueuedDeployments());
            }
            catch (Exception e)
            {
                logger.Error(e, "Error when running actions.");
            }

            logger.Debug("Completed action.");
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
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
                logger.Error(e, "Exception bubbling up in Windows service.");
            }

            logger.Info("Stopping Windows service.");
        }

        private static void CurrentDomain_UnhandledException(
            object sender,
            UnhandledExceptionEventArgs e)
        {
            var logger = Bootstrapper.Resolve<ILogger>();
            var exceptionObject = e.ExceptionObject as Exception;
            if (exceptionObject != null)
                logger.Error(exceptionObject, "Unhandled exception in Windows service.");
            else
                logger.Error("Unhandled exception in Windows service.");
        }
    }
}