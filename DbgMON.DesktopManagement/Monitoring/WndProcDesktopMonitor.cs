using System;
using System.Windows.Forms;

namespace DbgMON.DesktopManagement.Monitoring
{
    /// <summary>
    /// A NativeWindow that monitors 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.NativeWindow" />
    /// <seealso cref="DbgMON.DesktopManagement.Monitoring.IDesktopMonitor" />
    public class WndProcDesktopMonitor : NativeWindow, IDesktopMonitor
    {
        /// <summary>
        /// Occurs when [desktop background changed].
        /// </summary>
        public event EventHandler<DesktopBackgroundChangedEventArgs> DesktopBackgroundChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="WndProcDesktopMonitor"/> class.
        /// </summary>
        public WndProcDesktopMonitor()
        {
            CreateHandle(new CreateParams());
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            DestroyHandle();
        }

        /// <summary>
        /// Invokes the default window procedure associated with this window.
        /// </summary>
        /// <param name="m">A <see cref="T:System.Windows.Forms.Message" /> that is associated with the current Windows message.</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WindowsConstants.UserPreferenceChanged)
            {
                DesktopBackgroundChanged?.Invoke(this, new DesktopBackgroundChangedEventArgs()
                {
                    OldBackground = "boop",
                    NewBackground = "beep"
                });
            }

            base.WndProc(ref m);
        }
    }
}