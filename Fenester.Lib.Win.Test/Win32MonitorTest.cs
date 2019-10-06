using Fenester.Lib.Core.Service;
using Fenester.Lib.Test.Tools.Win;
using Fenester.Lib.Win.Service.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fenester.Lib.Win.Test
{
    [TestClass]
    public class Win32MonitorTest : DebuggableTest
    {
        protected override void InitTraces()
        {
            Win32Monitor.Tracable = this;
        }

        protected override void UninitTraces()
        {
            Win32Monitor.Tracable = null;
        }

        [TestMethod]
        public void GetMonitorsTest()
        {
            TraceFile.SetName("GetMonitorsTest");
            var screens = Win32Monitor.GetMonitors();
            foreach (var screen in screens)
            {
                this.LogLine("Monitor[{0}][{1}]", screen.Id, screen.Rectangle.Canonical);
            }
            Assert.IsTrue(true);
        }
    }
}