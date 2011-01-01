namespace Uncas.BuildPipeline.WindowsService
{
    using System.ComponentModel;

    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}