namespace DbgMON.DesktopManagement.Monitoring
{
    /// <summary>
    /// The desktop monitor factory
    /// </summary>
    public class DesktopMonitorFactory
    {
        /// <summary>
        /// Gets the desktop monitor.
        /// </summary>
        /// <returns>a desktop monitor</returns>
        public IDesktopMonitor GetDesktopMonitor(DesktopMonitorType monitorType)
        {
            switch (monitorType)
            {
                case DesktopMonitorType.Form:
                    return new HiddenFormDesktopMonitor();

                case DesktopMonitorType.NativeWindow:
                    return new DesktopMonitor();

                case DesktopMonitorType.WndProcNativeWindow:
                    return new WndProcDesktopMonitor();
            }

            return null;
        }
    }
}