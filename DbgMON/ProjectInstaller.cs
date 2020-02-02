using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace DbgMON
{
    /// <summary>
    /// Enables the ability to install this service
    /// </summary>
    /// <seealso cref="System.Configuration.Install.Installer" />
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectInstaller"/> class.
        /// </summary>
        public ProjectInstaller()
        {
            InitializeComponent();

            ServiceInstaller.Description = "watching u hehe...";
            ProcessInstaller.Account = ServiceAccount.LocalSystem;
        }
    }
}