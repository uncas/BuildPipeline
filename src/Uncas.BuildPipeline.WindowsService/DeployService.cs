namespace Uncas.BuildPipeline.WindowsService
{
    using System.ServiceProcess;
    using System.Timers;

    public partial class DeployService : ServiceBase
    {
        private readonly Timer timer;

        public DeployService()
        {
            InitializeComponent();
            const int intervalSeconds = 10;
            this.timer = new Timer(intervalSeconds * 1000);
            this.timer.Elapsed += 
                new ElapsedEventHandler(timer_Elapsed);
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
        }
    }
}