using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Graphical.Domain.Graphical;
using Fenester.Lib.Test.Tools.Win;
using Fenester.Lib.Win.Service;
using Fenester.Lib.Win.Service.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Fenester.Lib.Win.Test
{
    [TestClass]
    public class WindowOsServiceTest : DebuggableTest<WindowOsService, IWindowOsService>
    {
        protected override IEnumerable<ITracable> Tracables => base.Tracables;

        [TestMethod]
        public void GetWindowsSyncTest()
        {
            TraceFile.SetName("GetWindowsSyncTest");

            var windows = ServiceImpl.GetWindowsSync();

            foreach (var window in windows)
            {
                this.LogLine
                (
                    "{0}:[{1}] ({2}) [{3}]",
                    window.Canonical,
                    window.Title,
                    window.Class,
                    window.OsVisibility
                );
            }
        }

        private IEnumerable<IWindow> GetTestForms()
            => ServiceImpl
                .GetWindowsSync()
                .Where(window => window.Class == "TFormMain")
            ;

        [TestMethod]
        public void GetWindowsSyncFilterTest()
        {
            TraceFile.SetName("GetWindowsSyncFilterTest");

            var windows = GetTestForms();
            foreach (var window in windows)
            {
                this.LogLine
                (
                    "{0}:[{1}] ({2}) [{3}]",
                    window.Canonical,
                    window.Title,
                    window.Class,
                    window.OsVisibility
                );
            }
        }

        [TestMethod]
        public void HideSyncTest()
        {
            TraceFile.SetName("HideSyncTest");

            var window = GetTestForms().First();
            Assert.IsNotNull(ServiceImpl.HideSync(window));
            ServiceImpl.UnmanageSync(window);
        }

        [TestMethod]
        public void ShowSyncTest()
        {
            TraceFile.SetName("ShowSyncTest");

            var window = GetTestForms().First();
            Assert.IsNotNull(ServiceImpl.ShowSync(window));
            ServiceImpl.UnmanageSync(window);
        }

        [TestMethod]
        public void MoveSyncTest()
        {
            TraceFile.SetName("MoveSyncTest");

            var window = GetTestForms().First();
            Assert.IsNotNull(ServiceImpl.MoveSync(window, new Rectangle(800, 600, 2000, 200)));
            ServiceImpl.FocusWindowSync(window);
            Thread.Sleep(5000);
            ServiceImpl.UnmanageSync(window);
        }

        protected override void CreateServices()
        {
            ServiceImpl = new WindowOsService();
        }

        protected override void InitTraces()
        {
            Win32Window.Tracable = this;
        }

        protected override void UninitTraces()
        {
            Win32Window.Tracable = null;
        }
    }
}