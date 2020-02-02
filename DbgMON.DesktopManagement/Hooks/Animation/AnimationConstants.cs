using System;
using System.IO;

namespace DbgMON.DesktopManagement.Hooks.Animation
{
    /// <summary>
    /// Constants for animation
    /// </summary>
    public static class AnimationConstants
    {
        /// <summary>
        /// The animation message, used to build the slides
        /// </summary>
        public const string AnimationMessage = "hey you, stop that";

        /// <summary>
        /// The animation typing delay
        /// </summary>
        public const int AnimationTypingDelay = 50;

        /// <summary>
        /// The angery face delay
        /// </summary>
        public const int AngeryFaceDelay = 5000;

        /// <summary>
        /// The resources directory
        /// </summary>
        public static readonly string ResourcesDirectory = Path.Combine(Environment.CurrentDirectory, "Resources", @"DesktopBackgroundAnimationHook\");

        /// <summary>
        /// Gets the blank filename.
        /// </summary>
        public static readonly string BlankFilename = Path.Combine(ResourcesDirectory, "empty.png");

        /// <summary>
        /// The cursor filename
        /// </summary>
        public static readonly string CursorFilename = Path.Combine(ResourcesDirectory, "cursor-only.png");

        /// <summary>
        /// The angery face filename
        /// </summary>
        public static readonly string AngeryFaceFilename = Path.Combine(ResourcesDirectory, "angery.png");
    }
}