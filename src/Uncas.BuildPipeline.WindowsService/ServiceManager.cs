namespace Uncas.BuildPipeline.WindowsService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration.Install;
    using System.Diagnostics;
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
        UnInstall,
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
            _serviceName = serviceName;
            InitializeCommands();
        }

        protected string _serviceName { get; set; }

        public void RunCommand(ServiceManagerCommand command)
        {
            if (_commands.ContainsKey(command))
            {
                _commands[command]();
            }
        }

        public virtual bool IsServiceInstalled()
        {
            using (ServiceController serviceController =
                new ServiceController(_serviceName))
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
            using (ServiceController serviceController = new ServiceController(_serviceName))
            {
                if (!IsServiceInstalled())
                {
                    return false;
                }

                return serviceController.Status == ServiceControllerStatus.Running;
            }
        }

        protected virtual void InstallService()
        {
            if (IsServiceInstalled())
            {
                return;
            }

            try
            {
                string[] commandLine = new string[1];
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

        protected virtual void UninstallService()
        {
            if (!IsServiceInstalled())
            {
                return;
            }

            string[] commandLine = new string[1];
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

        protected virtual void StartService()
        {
            if (!IsServiceInstalled())
            {
                return;
            }

            using (ServiceController serviceController = new ServiceController(_serviceName))
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

            using (ServiceController serviceController = new ServiceController(_serviceName))
            {
                if (serviceController.Status != ServiceControllerStatus.Running)
                    return;
                serviceController.Stop();
                WaitForStatusChange(serviceController, ServiceControllerStatus.Stopped);
            }
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
                throw new Exception("Failed to change status of service. New status: " + newStatus);
            }
        }

        private void InitializeCommands()
        {
            _commands = new Dictionary<ServiceManagerCommand, Action>
            {
                {ServiceManagerCommand.Install, InstallService},
                {ServiceManagerCommand.UnInstall, UninstallService},
                {ServiceManagerCommand.Start, StartService},
                {ServiceManagerCommand.Stop, StopService},
            };
        }

        private AssemblyInstaller GetAssemblyInstaller(string[] commandLine)
        {
            AssemblyInstaller installer = new AssemblyInstaller();
            installer.Path = Environment.GetCommandLineArgs()[0];
            installer.CommandLine = commandLine;
            installer.UseNewContext = true;
            return installer;
        }
    }
}