using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbgMON.DesktopManagement.Monitoring
{
    /// <summary>
    /// The message pump's papa
    /// </summary>
    public class MessagePumpsDaddy : IDisposable
    {
        /// <summary>
        /// The disposed
        /// </summary>
        private bool _disposed = false; // To detect redundant calls

        /// <summary>
        /// The thread the message pump will run on
        /// </summary>
        private Thread _messagePumpThread;

        /// <summary>
        /// The initialized
        /// </summary>
        private ManualResetEvent _initialized = new ManualResetEvent(false);

        /// <summary>
        /// The monitor factory
        /// </summary>
        private DesktopMonitorFactory _monitorFactory;

        /// <summary>
        /// Gets Dave, the message pump.
        /// </summary>
        internal IDesktopMonitor Dave { get; private set; }

        /// <summary>
        /// Initialize the message pump thread
        /// </summary>
        public Task InitializeAsync(DesktopMonitorFactory monitorFactory)
        {
            _monitorFactory = monitorFactory;
            _messagePumpThread = new Thread(MessagePumpLifeSupport)
            {
                IsBackground = true,
            };

            _messagePumpThread.Start();
            return Task.Run(() => _initialized.WaitOne());
        }

        /// <summary>
        /// kill dave
        /// </summary>
        public void Filicide()
        {
            Dave?.Dispose();
            Dave = null;

            _messagePumpThread.Abort();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Keeps the message pump alive
        /// </summary>
        private void MessagePumpLifeSupport()
        {
            Dave = _monitorFactory.GetDesktopMonitor(DesktopMonitorType.WndProcNativeWindow);
            Application.ThreadException += (s, e) => Application.Exit();

            _initialized.Set();

            if (Dave is Form megaDave)
            {
                Application.Run(megaDave);
            }
            else
            {
                Application.Run();            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _initialized.Dispose();
                    Filicide();
                }

                _disposed = true;
            }
        }
    }
}