namespace DbgMON.DesktopManagement.Monitoring
{
    /// <summary>
    /// Desktop monitor type
    /// </summary>
    public enum DesktopMonitorType
    {
        /// <summary>
        /// The default
        /// </summary>
        Default,

        /// <summary>
        /// The native window
        /// </summary>
        NativeWindow,

        /// <summary>
        /// The form
        /// </summary>
        Form,

        /// <summary>
        /// The WND proc native window
        /// </summary>
        WndProcNativeWindow
    }
}