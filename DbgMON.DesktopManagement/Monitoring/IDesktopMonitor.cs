using System;
using System.Windows.Forms;

namespace DbgMON.DesktopManagement.Monitoring
{
    /// <summary>
    /// defines a desktop background monitor
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="System.Windows.Forms.IWin32Window" />
    public interface IDesktopMonitor : IDisposable, IWin32Window
    {
        event EventHandler<DesktopBackgroundChangedEventArgs> DesktopBackgroundChanged;
    }
}