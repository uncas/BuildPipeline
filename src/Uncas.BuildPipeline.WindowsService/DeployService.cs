namespace Uncas.BuildPipeline.WindowsService
{
    using System.ServiceProcess;
    using System.Timers;
    using Uncas.BuildPipeline.ApplicationServices;

    public partial class DeployService : ServiceBase
    {
        private readonly IDeploymentService _deploymentService;
        private readonly Timer _timer;

        public DeployService()
        {
            InitializeComponent();
            const int intervalSeconds = 10;
            _timer = new Timer(intervalSeconds * 1000);
            _timer.Elapsed +=
                timer_Elapsed;
            _deploymentService = Bootstrapper.Resolve<IDeploymentService>();
        }

        protected override void OnStart(string[] args)
        {
            _timer.Start();
        }

        protected override void OnStop()
        {
            _timer.Stop();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _deploymentService.DeployDueDeployments();
        }
    }
}