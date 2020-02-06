using DbgMON.DesktopManagement.Hooks;
using DbgMON.DesktopManagement.Monitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace DbgMON.DesktopManagement
{
    /// <summary>
    /// Desktop singleton
    /// </summary>
    /// <seealso cref="System.IDisposable" />
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
        /// The permanent set flags
        /// </summary>
        private const uint _permanentSetFlags = WindowsConstants.SendWinIniChange | WindowsConstants.UpdateIniFile;

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
        /// The dave
        /// </summary>
        private IDesktopMonitor _dave;

        /// <summary>
        /// The hooks
        /// </summary>
        private List<DesktopSecurityHook> _hooks = new List<DesktopSecurityHook>();

        /// <summary>
        /// The desktop set event
        /// </summary>
        private AutoResetEvent _desktopSet = new AutoResetEvent(false);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static Desktop Current { get; private set; }

        /// <summary>
        /// Gets or sets the factory.
        /// </summary>
        public static DesktopMonitorFactory Factory { get; set; } = new DesktopMonitorFactory();

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="monitorType">The monitor.</param>
        /// <returns>
        /// The Desktop singleton
        /// </returns>
        public static Desktop Initialize(DesktopMonitorType monitorType = DesktopMonitorType.Default)
        {
            if (Current == null)
            {
                if (monitorType != DesktopMonitorType.Default)
                {
                    var monitor = Factory.GetDesktopMonitor(monitorType);
                    Current = new Desktop(monitor);
                }
                else
                {
                    Current = new Desktop();
                }
            }

            return Current;
        }

        /// <summary>
        /// Initializes the specified testable.
        /// </summary>
        /// <param name="testable">if set to <c>true</c> [testable].</param>
        /// <param name="monitorType">The monitor type.</param>
        /// <returns>The current Desktop instance</returns>
        public static Desktop Initialize(bool testable, DesktopMonitorType monitorType = DesktopMonitorType.Default)
        {
            var desktop = Initialize(monitorType);
            desktop.Testable = testable;
            return desktop;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Desktop"/> is testable.
        /// </summary>
        public bool Testable { get; private set; }

        /// <summary>
        /// Gets the last duration of the call to SetDesktopBackground.
        /// Only set if this instance was initialized as testable.
        /// </summary>
        public TimeSpan LastSetDesktopCallDuration { get; private set; }

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
        public Task<bool> SetDesktopBackground(string filename)
        {
            if (_isInternalSet)
            {
                return Task.FromResult(false);
            }

            _isInternalSet = true;


            if (!SystemParametersInfo(WindowsConstants.SetDesktopBackgroundCommand, 0, filename, _permanentSetFlags))
            {
                _isInternalSet = false;
                return Task.FromResult(false);
            }

            return Task.Run(_desktopSet.WaitOne);
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
                if (_randy?.Dave != null)
                {
                    _randy.Dave.DesktopBackgroundChanged -= OnDesktopBackgroundChanged;
                }

                _randy?.Dispose();
                _randy = null;

                if (_dave != null)
                {
                    _dave.DesktopBackgroundChanged -= OnDesktopBackgroundChanged;
                    _dave?.Dispose();
                    _dave = null;
                }
            }

            _disposed = true;
            Current = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Desktop"/> class.
        /// </summary>
        private Desktop()
        {
            _randy = new MessagePumpsDaddy();
            Task.Run(async () =>
            {
                await _randy.InitializeAsync(Factory);
                _randy.Dave.DesktopBackgroundChanged += OnDesktopBackgroundChanged;
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Desktop"/> class.
        /// </summary>
        /// <param name="monitor">The monitor.</param>
        private Desktop(IDesktopMonitor monitor)
        {
            _dave = monitor;
            _dave.DesktopBackgroundChanged += OnDesktopBackgroundChanged;
        }

        /// <summary>
        /// Called when [desktop background changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DesktopBackgroundChangedEventArgs"/> instance containing the event data.</param>
        private void OnDesktopBackgroundChanged(object sender, DesktopBackgroundChangedEventArgs e)
        {
            if (_isInternalSet)
            {
                _desktopSet.Set();
                _isInternalSet = false;
            }
            else
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
}