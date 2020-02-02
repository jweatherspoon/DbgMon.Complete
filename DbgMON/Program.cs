using System.ServiceProcess;

namespace DbgMON
{
    /// <summary>
    /// The program
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new DbgMONService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}