using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace DbgMON.DesktopManagement.Monitoring
{
    /// <summary>
    /// Monitors the desktop for changes to its background
    /// </summary>
    /// <seealso cref="System.Windows.Forms.NativeWindow" />
    public class DesktopMonitor : NativeWindow, IDisposable, IDesktopMonitor
    {
        /// <summary>
        /// prevents redundant dispose calls
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// The previous background
        /// </summary>
        private string _previousBackground = "always lit";

        /// <summary>
        /// Occurs when [desktop background changed].
        /// </summary>
        public event EventHandler<DesktopBackgroundChangedEventArgs> DesktopBackgroundChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopMonitor"/> class.
        /// </summary>
        public DesktopMonitor()
        {
            CreateHandle(new CreateParams());

            SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                SystemEvents.UserPreferenceChanged -= OnUserPreferenceChanged;
                DestroyHandle();
            }

            _disposed = true;
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
                var currentBackground = GetDesktopBackgroundFilename();
                DesktopBackgroundChanged?.Invoke(this, new DesktopBackgroundChangedEventArgs()
                {
                    OldBackground = _previousBackground,
                    NewBackground = currentBackground
                });
            }
        }

        /// <summary>
        /// Gets the desktop background filename.
        /// </summary>
        /// <returns></returns>
        private string GetDesktopBackgroundFilename()
        {
            return Guid.NewGuid().ToString();
        }
    }
}