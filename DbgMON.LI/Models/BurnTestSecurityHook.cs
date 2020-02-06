using DbgMON.DesktopManagement.Hooks;
using DbgMON.DesktopManagement.Hooks.Animation;
using DbgMON.DesktopManagement.Monitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbgMON.LI.Models
{
    public class BurnTestSecurityHook : DesktopBackgroundAnimationHook
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BurnTestSecurityHook"/> class.
        /// </summary>
        public BurnTestSecurityHook() : base(GetTestSlides())
        {
        }

        /// <summary>
        /// Executes the specified e.
        /// </summary>
        /// <param name="e">The <see cref="T:DbgMON.DesktopManagement.Monitoring.DesktopBackgroundChangedEventArgs" /> instance containing the event data.</param>
        /// <returns></returns>
        public override Task Execute(DesktopBackgroundChangedEventArgs e)
        {
            return default;
        }

        private static IEnumerable<AnimationSlide> GetTestSlides()
        {
            yield break;
        }
    }
}