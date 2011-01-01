namespace Uncas.BuildPipeline.WindowsService
{
    using System.ServiceProcess;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new DeployService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}