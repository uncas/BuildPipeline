namespace Uncas.BuildPipeline.WindowsService
{
    using System.ServiceProcess;
    using System.Timers;
    using Uncas.BuildPipeline.ApplicationServices;

    public partial class DeployService : ServiceBase
    {
        private readonly Timer timer;
        private IDeploymentService deploymentService;

        public DeployService()
        {
            InitializeComponent();
            const int intervalSeconds = 10;
            this.timer = new Timer(intervalSeconds * 1000);
            this.timer.Elapsed +=
                new ElapsedEventHandler(timer_Elapsed);
            this.deploymentService = Bootstrapper.GetDeploymentService();
        }

        protected override void OnStart(string[] args)
        {
            this.timer.Start();
        }

        protected override void OnStop()
        {
            this.timer.Stop();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.deploymentService.DeployDueDeployments();
        }
    }
}