namespace DbgMON.DesktopManagement.Monitoring
{
    public class DesktopBackgroundChangedEventArgs
    {
        /// <summary>
        /// Gets or sets the old background.
        /// </summary>
        public string OldBackground { get; set; }

        /// <summary>
        /// Creates new background.
        /// </summary>
        public string NewBackground { get; set; }

        /// <summary>
        /// Gets a value indicating whether the background has actually changed
        /// </summary>
        public bool IsDifferent => OldBackground != NewBackground;
    }
}