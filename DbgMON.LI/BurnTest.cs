using DbgMON.DesktopManagement;
using DbgMON.DesktopManagement.Hooks;
using DbgMON.DesktopManagement.Monitoring;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace DbgMON.LI
{
    /// <summary>
    /// Long term burn test to ensure system doesn't crash after continued use
    /// </summary>
    [TestClass]
    public class DbgMONBurnTest
    {
        /// <summary>
        /// The desktop under test
        /// </summary>
        private Desktop _desktop;

        /// <summary>
        /// The monitor
        /// </summary>
        private Mock<IDesktopMonitor> _monitorMock;

        /// <summary>
        /// The test hook
        /// </summary>
        private DesktopSecurityHook _testHook;

        /// <summary>
        /// test init
        /// </summary>
        [TestInitialize]
        public void BeforeEachTest()
        {
            _monitorMock = new Mock<IDesktopMonitor>();
            //_desktop = Desktop.Initialize(true, _monitorMock.Object);
        }

        /// <summary>
        /// Burns the test. sure
        /// </summary>
        [Timeout(60000 * 24)]
        public void BurnTest()
        {
            // Setup
            
            _desktop.AddSecurityHook(_testHook);
        }
    }
}