using Fenester.Lib.Win.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fenester.Lib.Win.Test
{
    [TestClass]
    public class Win32MonitorTest
    {
        [TestMethod]
        public void GetMonitorsTest()
        {
            var screens = Win32Monitor.GetMonitors();
            TraceFile
                .Get("Monitors")
                .Each
                (
                    screens,
                    (trace, screen) => trace.Out("Monitor[{0}][{1}]", screen.Id, screen.Rectangle.Canonical)
                )
                .Close()
            ;
            Assert.IsTrue(true);
        }
    }
}