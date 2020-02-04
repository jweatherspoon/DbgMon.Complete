using DbgMON.DesktopManagement;
using DbgMON.DesktopManagement.Hooks;
using DbgMON.DesktopManagement.Hooks.Animation;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace DbgMON
{
    /// <summary>
    /// Windows service that monitors and protects the desktop background
    /// </summary>
    /// <seealso cref="System.ServiceProcess.ServiceBase" />
    public partial class DbgMONService : ServiceBase
    {
        /// <summary>
        /// The hooks
        /// </summary>
        private static DesktopSecurityHook[] _hooks = new DesktopSecurityHook[]
        {
            new DesktopBackgroundAnimationHook(GetAnimationSlides()),
            new DesktopRestoreHook(DesktopRestoreHook.Background, 5000)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="DbgMONService"/> class.
        /// </summary>
        public DbgMONService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            var desktop = Desktop.Initialize();
            foreach (var hook in _hooks)
            {
                desktop.AddSecurityHook(hook);
            }
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
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
                yield return CreateAnimationSlide(filenameBuilder.ToString());
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