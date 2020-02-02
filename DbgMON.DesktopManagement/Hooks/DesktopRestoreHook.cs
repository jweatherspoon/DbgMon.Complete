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
        private const string Background = @"C:\Users\ItsAn\Pictures\IMG_1293.png";

        /// <summary>
        /// The delay time in milliseconds
        /// </summary>
        private const int DelayTimeMs = 5000;

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="e">The <see cref="DesktopBackgroundChangedEventArgs" /> instance containing the event data.</param>
        public override async Task Execute(DesktopBackgroundChangedEventArgs e)
        {
            await Task.Delay(DelayTimeMs);
            await _desktop.SetDesktopBackground(Background);
        }
    }
}