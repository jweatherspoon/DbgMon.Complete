namespace DbgMON.DesktopManagement
{
    /// <summary>
    /// Contains windows constants 
    /// </summary>
    internal class WindowsConstants
    {
        /// <summary>
        /// The user preference changed
        /// </summary>
        public const int UserPreferenceChanged = 0x001A;

        /// <summary>
        /// The command for setting a desktop background
        /// </summary>
        public const int SetDesktopBackgroundCommand = 20;

        /// <summary>
        /// The spif updateinifile
        /// </summary>
        public const int UpdateIniFile = 0x01;

        /// <summary>
        /// The spif sendwininichange
        /// </summary>
        public const int SendWinIniChange = 0x02;
    }
}