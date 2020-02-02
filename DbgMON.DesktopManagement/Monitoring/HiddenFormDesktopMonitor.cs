using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace DbgMON.DesktopManagement.Monitoring
{
    /// <summary>
    /// Desktop monitor that inherits from Form instead of NativeWindow
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    /// <seealso cref="DbgMON.DesktopManagement.Monitoring.IDesktopMonitor" />
    public class HiddenFormDesktopMonitor : Form, IDesktopMonitor
    {
        /// <summary>
        /// The disposed flag
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// Occurs when [desktop background changed].
        /// </summary>
        public event EventHandler<DesktopBackgroundChangedEventArgs> DesktopBackgroundChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="HiddenFormDesktopMonitor"/> class.
        /// </summary>
        public HiddenFormDesktopMonitor()
        {
            SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
        }

        /// <summary>
        /// Called when [user preference changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="UserPreferenceChangedEventArgs"/> instance containing the event data.</param>
        private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.Desktop)
            {
                DesktopBackgroundChanged?.Invoke(this, new DesktopBackgroundChangedEventArgs()
                {
                    OldBackground = "this",
                    NewBackground = "that"
                });
            }
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.Form" />.
        /// </summary>
        /// <param name="disposing"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!_disposed && disposing)
                {
                    SystemEvents.UserPreferenceChanged -= OnUserPreferenceChanged;
                }

                base.Dispose(disposing);
            }
            catch
            {
                /**
                 * Only happens when closing, so whatever I guess.
                 * Maybe I'll fix it in the future; if I do future me:
                 * The problem is that this guy can't be disposed on any other 
                 * thread than the one he is created on. So you just gotta 
                 * implement some way of destroying him from that thread. lame
                 */
            }
        }
    }
}