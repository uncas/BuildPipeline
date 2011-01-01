namespace Uncas.BuildPipeline.WindowsService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.ServiceProcess;

    internal static class Program
    {
        /// <summary>
        /// Name of the service as defined in the Service component.
        /// </summary>
        private const string ServiceName = "DeployService";
        /// <summary>
        /// Mapping of console command line args to ServiceManagerCommands.
        /// </summary>
        private static Dictionary<string, ServiceManagerCommand> _commands = new Dictionary<string, ServiceManagerCommand>
        {
            {"-console", ServiceManagerCommand.Application},
            {"-install", ServiceManagerCommand.Install},
            {"-uninstall", ServiceManagerCommand.UnInstall},
            {"-start", ServiceManagerCommand.Start},
            {"-stop", ServiceManagerCommand.Stop}
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
                ServiceManager serviceManager = new ServiceManager(ServiceName);
                if (command == ServiceManagerCommand.Application)
                {
                    Console.WriteLine("Running in console mode.");
                    // TODO: Call service method here.
                    Console.Read();
                }
                else
                {
                    serviceManager.RunCommand(command);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("DeployService",
                                    ex.ToString(), EventLogEntryType.Error);
            }
        }

        private static void ServiceMain()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
                            {
                                new DeployService()
                            };
            ServiceBase.Run(ServicesToRun);
        }

        private static bool TryParseCommandLine(string[] args,
                                                out ServiceManagerCommand command)
        {
            command = ServiceManagerCommand.Unknown;
            if (args.Length > 1)
                return false;
            string commandLineArg = args[0];
            if (_commands.ContainsKey(commandLineArg))
            {
                command = _commands[commandLineArg];
                return true;
            }
            return false;
        }

        private static void PrintUsage()
        {
            string exeName = Assembly.GetExecutingAssembly().ManifestModule.Name;
            Console.WriteLine("Usage:");
            foreach (var item in _commands)
            {
                Console.WriteLine("  " + exeName + " " + item.Key);
            }
            Console.Read();
        }
    }
}