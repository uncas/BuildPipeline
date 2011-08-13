namespace Uncas.BuildPipeline.WindowsService
{
    using System.ServiceProcess;
    using System.Timers;
    using Uncas.BuildPipeline.ApplicationServices;

    /// <summary>
    /// Service for deploying.
    /// </summary>
    public partial class DeployService : ServiceBase
    {
        private readonly IDeploymentService _deploymentService;
        private readonly Timer _timer;

        public DeployService()
        {
            InitializeComponent();
            const int IntervalSeconds = 10;
            _timer = new Timer(IntervalSeconds * 1000);
            _timer.Elapsed +=
                TimerElapsed;
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

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _deploymentService.DeployDueDeployments();
        }
    }
}