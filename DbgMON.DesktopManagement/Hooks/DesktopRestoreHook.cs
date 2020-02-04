using DbgMON.DesktopManagement.Monitoring;
using System.Threading.Tasks;

namespace DbgMON.DesktopManagement.Hooks
{
    /// <summary>
    /// Restores the previous desktop background after someone tries to change it
    /// </summary>
    /// <seealso cref="DbgMON.DesktopManagement.Hooks.DesktopSecurityHook" />
    public class DesktopRestoreHook : DesktopSecurityHook
    {
        /// <summary>
        /// Gets the background.
        /// </summary>
        public const string Background = @"C:\Users\ItsAn\Pictures\IMG_1293.png";

        /// <summary>
        /// The delay time in milliseconds
        /// </summary>
        private int _delayTimeMs = 5000;

        /// <summary>
        /// The background to restore
        /// </summary>
        private string _background;

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopRestoreHook"/> class.
        /// </summary>
        /// <param name="backgroundToRestore">The background to restore.</param>
        /// <param name="delayTimeMs">The delay time in ms.</param>
        public DesktopRestoreHook(string backgroundToRestore, int delayTimeMs)
        {
            _background = backgroundToRestore;
            _delayTimeMs = delayTimeMs;
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="e">The <see cref="DesktopBackgroundChangedEventArgs" /> instance containing the event data.</param>
        public override async Task Execute(DesktopBackgroundChangedEventArgs e)
        {
            await Task.Delay(_delayTimeMs);
            await _desktop.SetDesktopBackground(_background);
        }
    }
}