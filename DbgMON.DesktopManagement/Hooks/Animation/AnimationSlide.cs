namespace DbgMON.DesktopManagement.Hooks.Animation
{
    /// <summary>
    /// Models a slide in an animation
    /// </summary>
    public class AnimationSlide
    {
        /// <summary>
        /// Gets or sets the path to the image file this slide will show.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the display time of this slide in milliseconds.
        /// </summary>
        public int DisplayTime { get; set; }
    }
}