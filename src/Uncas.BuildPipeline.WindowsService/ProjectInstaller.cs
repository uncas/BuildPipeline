namespace Uncas.BuildPipeline.WindowsService
{
    using System.ComponentModel;
    using System.Configuration.Install;

    /// <summary>
    /// Installer for the project.
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}