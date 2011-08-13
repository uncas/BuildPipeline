namespace Uncas.BuildPipeline.WindowsService
{
    using System.ComponentModel;

    /// <summary>
    /// Installer for the project.
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}