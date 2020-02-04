using DbgMON.DesktopManagement;
using DbgMON.DesktopManagement.Hooks;
using DbgMON.DesktopManagement.Hooks.Animation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbgMONConsole
{
    /// <summary>
    /// Used to manually test the DesktopManagement dll during development
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            Desktop.Initialize();

            var animationHook = new DesktopBackgroundAnimationHook(GetAnimationSlides());
            var restoreHook = new DesktopRestoreHook(DesktopRestoreHook.Background, 5000);
            Desktop.Current.AddSecurityHook(animationHook);
            Desktop.Current.AddSecurityHook(restoreHook);

            Console.ReadLine();

            Desktop.Current.Dispose();
        }

        /// <summary>
        /// Gets the animation slides.
        /// </summary>
        /// <returns>the animation slides</returns>
        private static IEnumerable<AnimationSlide> GetAnimationSlides()
        {
            // Start out with a blinking cursor
            var blinkingCursor = new List<AnimationSlide>()
            {
                CreateAnimationSlide(AnimationConstants.CursorFilename),
                CreateAnimationSlide(AnimationConstants.BlankFilename)
            };

            // (cursor, blank, cursor, blank, cursor, blank)
            var index = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var slide = blinkingCursor[index];
                    index = (index + 1) % 1;
                    yield return slide;
                }
            }

            // Then type out our AnimationMessage
            var filenameBuilder = new StringBuilder(AnimationConstants.ResourcesDirectory);
            foreach (var character in AnimationConstants.AnimationMessage)
            {
                filenameBuilder.Append(character);
                yield return CreateAnimationSlide($"{filenameBuilder.ToString()}.png");
            }

            // Then add the angery fais
            yield return CreateAnimationSlide(AnimationConstants.AngeryFaceFilename);
        }

        /// <summary>
        /// Creates the animation slide.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>An animation slide</returns>
        private static AnimationSlide CreateAnimationSlide(string filename)
        {
            return new AnimationSlide()
            {
                DisplayTime = AnimationConstants.AnimationTypingDelay,
                Filename = filename
            };
        }
    }
}