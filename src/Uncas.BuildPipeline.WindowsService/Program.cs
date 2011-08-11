namespace Uncas.BuildPipeline.WindowsService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.ServiceProcess;
    using Uncas.BuildPipeline.ApplicationServices;

    internal static class Program
    {
        /// <summary>
        /// Name of the service as defined in the Service component.
        /// </summary>
        private const string ServiceName = "DeployService";

        /// <summary>
        /// Mapping of console command line args to ServiceManagerCommands.
        /// </summary>
        private static readonly Dictionary<string, ServiceManagerCommand> _commands =
            new Dictionary<string, ServiceManagerCommand>
                {
                    { "-console", ServiceManagerCommand.Application },
                    { "-install", ServiceManagerCommand.Install },
                    { "-uninstall", ServiceManagerCommand.Uninstall },
                    { "-start", ServiceManagerCommand.Start },
                    { "-stop", ServiceManagerCommand.Stop }
                };

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            try
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                if (args.Length == 0)
                {
                    ServiceMain();
                    return;
                }

                ServiceManagerCommand command;
                if (!TryParseCommandLine(args, out command))
                {
                    PrintUsage();
                    return;
                }

                var serviceManager = new ServiceManager(ServiceName);
                if (command == ServiceManagerCommand.Application)
                {
                    const string message = @"Running in console mode.";
                    Console.WriteLine(message);
                    Bootstrapper.Resolve<IDeploymentService>().DeployDueDeployments();
                    Console.Read();
                }
                else
                {
                    serviceManager.RunCommand(command);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(
                    "DeployService",
                    ex.ToString(),
                    EventLogEntryType.Error);
            }
        }

        private static void PrintUsage()
        {
            string exeName = Assembly.GetExecutingAssembly().ManifestModule.Name;
            Console.WriteLine("Usage:");
            foreach (var item in _commands)
            {
                Console.WriteLine(@"  " + exeName + @" " + item.Key);
            }

            Console.Read();
        }

        private static void ServiceMain()
        {
            var ServicesToRun = new ServiceBase[]
                                    {
                                        new DeployService()
                                    };
            ServiceBase.Run(ServicesToRun);
        }

        private static bool TryParseCommandLine(
            string[] args,
            out ServiceManagerCommand command)
        {
            command = ServiceManagerCommand.Unknown;
            if (args.Length > 1)
            {
                return false;
            }

            string commandLineArg = args[0];
            if (_commands.ContainsKey(commandLineArg))
            {
                command = _commands[commandLineArg];
                return true;
            }

            return false;
        }
    }
}