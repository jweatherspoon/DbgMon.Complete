using DbgMON.DesktopManagement.Hooks.Animation;
using DbgMON.DesktopManagement.Monitoring;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace DbgMON.DesktopManagement.Hooks
{
    /// <summary>
    /// Displays an animation by setting the desktop background repeatedly
    /// </summary>
    /// <seealso cref="DbgMON.DesktopManagement.Hooks.DesktopSecurityHook" />
    public class DesktopBackgroundAnimationHook : DesktopSecurityHook
    {
        /// <summary>
        /// The animation slides
        /// </summary>
        protected IEnumerable<AnimationSlide> _slides;

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopBackgroundAnimationHook"/> class.
        /// </summary>
        /// <param name="slides">The slides.</param>
        public DesktopBackgroundAnimationHook(IEnumerable<AnimationSlide> slides)
        {
            _slides = slides;
        }

        /// <summary>
        /// Executes the specified e.
        /// </summary>
        /// <param name="e">The <see cref="DesktopBackgroundChangedEventArgs" /> instance containing the event data.</param>
        public override async Task Execute(DesktopBackgroundChangedEventArgs e)
        {
            foreach (var slide in _slides)
            {
                if (!File.Exists(slide.Filename))
                {
                    Debug.WriteLine("What the frick where am I???", slide.Filename);
                    continue;
                }

                await Desktop.Current?.SetDesktopBackground(slide.Filename);
                await Task.Delay(slide.DisplayTime);
            }
        }
    }
}