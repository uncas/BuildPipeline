namespace Uncas.BuildPipeline.WindowsService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration.Install;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.ServiceProcess;
    using System.Threading;

    /// <summary>
    /// Commands that can be executed for a service.
    /// Note: Application is not really a service command, 
    /// but we let the client use it as a marker
    /// for not being in service mode.
    /// </summary>
    public enum ServiceManagerCommand
    {
        Unknown,
        Application,
        Install,
        Uninstall,
        Start,
        Stop,
    }

    /// <summary>
    /// Install, Uninstall, Start and Stop services.
    /// </summary>
    public class ServiceManager
    {
        private Dictionary<ServiceManagerCommand, Action> _commands;

        /// <summary>
        /// Initialize a ServiceManager.
        /// </summary>
        /// <param name="serviceName">
        /// The name of the service as defined in the Service component.
        /// </param>
        public ServiceManager(string serviceName)
        {
            ServiceName = serviceName;
            InitializeCommands();
        }

        private string ServiceName { get; set; }

        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Needs to be robust - general exceptions are logged.")]
        public virtual bool IsServiceInstalled()
        {
            using (var serviceController =
                new ServiceController(ServiceName))
            {
                try
                {
                    ServiceControllerStatus status = serviceController.Status;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(
                        "ServiceManager",
                        ex.ToString(),
                        EventLogEntryType.Error);
                    return false;
                }

                return true;
            }
        }

        public bool IsServiceRunning()
        {
            using (var serviceController = new ServiceController(ServiceName))
            {
                if (!IsServiceInstalled())
                {
                    return false;
                }

                return serviceController.Status == ServiceControllerStatus.Running;
            }
        }

        public void RunCommand(ServiceManagerCommand command)
        {
            if (_commands.ContainsKey(command))
            {
                _commands[command]();
            }
        }

        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Needs to be robust - general exceptions are logged.")]
        protected virtual void InstallService()
        {
            if (IsServiceInstalled())
            {
                return;
            }

            try
            {
                var commandLine = new string[1];
                commandLine[0] = "Test install";
                IDictionary mySavedState = new Hashtable();
                AssemblyInstaller installer = GetAssemblyInstaller(commandLine);
                try
                {
                    installer.Install(mySavedState);
                    installer.Commit(mySavedState);
                }
                catch (Exception ex)
                {
                    installer.Rollback(mySavedState);
                    EventLog.WriteEntry(
                        "ServiceManager",
                        ex.ToString(),
                        EventLogEntryType.Error);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("ServiceManager", ex.ToString());
            }
        }

        protected virtual void StartService()
        {
            if (!IsServiceInstalled())
            {
                return;
            }

            using (var serviceController = new ServiceController(ServiceName))
            {
                if (serviceController.Status == ServiceControllerStatus.Stopped)
                {
                    try
                    {
                        serviceController.Start();
                        WaitForStatusChange(
                            serviceController,
                            ServiceControllerStatus.Running);
                    }
                    catch (InvalidOperationException ex)
                    {
                        EventLog.WriteEntry(
                            "ServiceManager",
                            ex.ToString(),
                            EventLogEntryType.Error);
                    }
                }
            }
        }

        protected virtual void StopService()
        {
            if (!IsServiceInstalled())
            {
                return;
            }

            using (var serviceController = new ServiceController(ServiceName))
            {
                if (serviceController.Status != ServiceControllerStatus.Running)
                {
                    return;
                }
                serviceController.Stop();
                WaitForStatusChange(serviceController, ServiceControllerStatus.Stopped);
            }
        }

        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Needs to be robust - general exceptions are logged.")]
        protected virtual void UninstallService()
        {
            if (!IsServiceInstalled())
            {
                return;
            }

            var commandLine = new string[1];
            commandLine[0] = "Test Uninstall";
            IDictionary mySavedState = new Hashtable();
            mySavedState.Clear();
            AssemblyInstaller installer = GetAssemblyInstaller(commandLine);
            try
            {
                installer.Uninstall(mySavedState);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(
                    "ServiceManager",
                    ex.ToString(),
                    EventLogEntryType.Error);
            }
        }

        [SuppressMessage(
            "Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "Is disposed in other methods in this class.")]
        private static AssemblyInstaller GetAssemblyInstaller(string[] commandLine)
        {
            var installer = new AssemblyInstaller();
            installer.Path = Environment.GetCommandLineArgs()[0];
            installer.CommandLine = commandLine;
            installer.UseNewContext = true;
            return installer;
        }

        /// <summary>
        /// After a service has been installed, uninstalled, started or stopped,
        /// it might take some time
        /// for the action to complete. Wait here until we get the new status or time out.
        /// </summary>
        /// <param name="serviceController"></param>
        /// <param name="newStatus"></param>
        private static void WaitForStatusChange(
            ServiceController serviceController,
            ServiceControllerStatus newStatus)
        {
            int count = 0;
            while (serviceController.Status != newStatus && count < 30)
            {
                Thread.Sleep(1000);
                serviceController.Refresh();
                count++;
            }

            if (serviceController.Status != newStatus)
            {
                throw new InvalidOperationException("Failed to change status of service. New status: " + newStatus);
            }
        }

        private void InitializeCommands()
        {
            _commands = new Dictionary<ServiceManagerCommand, Action>
                            {
                                { ServiceManagerCommand.Install, InstallService },
                                { ServiceManagerCommand.Uninstall, UninstallService },
                                { ServiceManagerCommand.Start, StartService },
                                { ServiceManagerCommand.Stop, StopService },
                            };
        }
    }
}