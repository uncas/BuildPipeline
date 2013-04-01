using System;
using System.ServiceProcess;
using System.Timers;

namespace Uncas.BuildPipeline.WindowsService
{
    /// <summary>
    /// Service for deploying.
    /// </summary>
    public partial class DeployService : ServiceBase
    {
        private readonly Action _action;
        private readonly Timer _timer;

        public DeployService(Action action)
        {
            _action = action;
            InitializeComponent();
            const int intervalSeconds = 10;
            _timer = new Timer(intervalSeconds*1000);
            _timer.Elapsed +=
                TimerElapsed;
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
            _action();
        }
    }
}