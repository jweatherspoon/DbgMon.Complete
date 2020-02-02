using DbgMON.DesktopManagement.Monitoring;
using System.Threading.Tasks;

namespace DbgMON.DesktopManagement.Hooks
{
    /// <summary>
    /// Event hook that responds to a UserPreferenceChanged event
    /// </summary>
    public abstract class DesktopSecurityHook
    {
        /// <summary>
        /// Gets the current desktop.
        /// </summary>
        protected Desktop _desktop => Desktop.Current;

        /// <summary>
        /// Determines whether this instance can execute the specified e.
        /// </summary>
        /// <param name="e">The <see cref="DesktopBackgroundChangedEventArgs"/> instance containing the event data.</param>
        /// <returns>
        ///   <c>true</c> if this instance can execute the specified e; otherwise, <c>false</c>.
        /// </returns>
        public bool CanExecute(DesktopBackgroundChangedEventArgs e)
        {
            return e.NewBackground != e.OldBackground;
        }

        /// <summary>
        /// Executes the specified e.
        /// </summary>
        /// <param name="e">The <see cref="DesktopBackgroundChangedEventArgs"/> instance containing the event data.</param>
        public abstract Task Execute(DesktopBackgroundChangedEventArgs e);
    }
}