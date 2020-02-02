using DbgMON.DesktopManagement.Hooks;
using DbgMON.DesktopManagement.Monitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DbgMON.DesktopManagement
{
    /// <summary>
    /// Desktop singleton
    /// </summary>
    public sealed class Desktop : IDisposable
    {
        /// <summary>
        /// Sets a system parameter
        /// </summary>
        /// <param name="uiAction">The UI action.</param>
        /// <param name="uiParam">The UI parameter.</param>
        /// <param name="pvParam">The pv parameter.</param>
        /// <param name="fwinIni">The fwin ini.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fwinIni);

        /// <summary>
        /// The disposed
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// The is internal set flag
        /// </summary>
        private volatile bool _isInternalSet = false;

        /// <summary>
        /// handles the monitor window's lifecycle
        /// </summary>
        private MessagePumpsDaddy _randy;

        /// <summary>
        /// The hooks
        /// </summary>
        private List<DesktopSecurityHook> _hooks = new List<DesktopSecurityHook>();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static Desktop Current { get; private set; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>The Desktop singleton</returns>
        public static Desktop Initialize()
        {
            if (Current == null)
            {
                Current = new Desktop();
            }

            return Current;
        }

        /// <summary>
        /// Adds the security hook.
        /// </summary>
        /// <param name="hook">The hook.</param>
        public void AddSecurityHook(DesktopSecurityHook hook)
        {
            _hooks.Add(hook);
        }

        /// <summary>
        /// Sets the desktop background.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public async Task<bool> SetDesktopBackground(string filename)
        {
            if (_isInternalSet)
            {
                return false;
            }

            _isInternalSet = true;
            //var flags = WindowsConstants.SendWinIniChange | WindowsConstants.UpdateIniFile;
            var flags = 0u;
            if (!SystemParametersInfo(WindowsConstants.SetDesktopBackgroundCommand, 0, filename, flags))
            {
                _isInternalSet = false;
                return false;
            }

            await Task.Delay(200);
            _isInternalSet = false;
            return true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _randy.Dispose();
                _randy = null;
            }

            _disposed = true;
            Current = null;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Desktop"/> class from being created.
        /// </summary>
        private Desktop()
        {
            _randy = new MessagePumpsDaddy();
            Task.Run(async () =>
            {
                await _randy.InitializeAsync(new DesktopMonitorFactory(true));
                _randy.Dave.DesktopBackgroundChanged += OnDesktopBackgroundChanged;
            });
        }

        /// <summary>
        /// Called when [desktop background changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DesktopBackgroundChangedEventArgs"/> instance containing the event data.</param>
        private void OnDesktopBackgroundChanged(object sender, DesktopBackgroundChangedEventArgs e)
        {
            var executableHooks = _hooks.Where(x => x.CanExecute(e));
            Task.Run(async () =>
            {
                foreach (var hook in executableHooks)
                {
                    await hook.Execute(e);
                }
            });
        }
    }
}