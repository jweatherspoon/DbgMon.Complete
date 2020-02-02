namespace DbgMON.DesktopManagement.Monitoring
{
    /// <summary>
    /// The desktop monitor factory
    /// </summary>
    public class DesktopMonitorFactory
    {
        /// <summary>
        /// The is form flag
        /// </summary>
        private bool _isForm;

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopMonitorFactory"/> class.
        /// </summary>
        /// <param name="isForm">if set to <c>true</c> [is form].</param>
        public DesktopMonitorFactory(bool isForm)
        {
            _isForm = isForm;
        }

        /// <summary>
        /// Gets the desktop monitor.
        /// </summary>
        /// <returns>a desktop monitor</returns>
        internal IDesktopMonitor GetDesktopMonitor()
        {
            if (_isForm)
            {
                return new HiddenFormDesktopMonitor();
            }
            else
            {
                return new DesktopMonitor();
            }
        }
    }
}