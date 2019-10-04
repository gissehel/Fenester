using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Graphical.Domain.Graphical;
using Fenester.Lib.Win.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Fenester.Lib.Win.Test
{
    [TestClass]
    public class WindowOsServiceTest
    {
        private WindowOsService WindowOsService { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            WindowOsService = new WindowOsService();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            WindowOsService = null;
        }

        [TestMethod]
        public void GetWindowsSyncTest()
        {
            var windows = WindowOsService.GetWindowsSync();

            TraceFile
                .Get("GetWindowsSync")
                .Each(
                    windows,
                    (trace, window) => trace.Out
                        (
                            "{0}:[{1}] ({2}) [{3}]",
                            window.Canonical,
                            window.Title,
                            window.Class,
                            window.OsVisibility
                        )
                )
                .Close()
            ;
        }

        private IEnumerable<IWindow> GetTestForms()
            => WindowOsService
                .GetWindowsSync()
                .Where(window => window.Class == "TFormMain")
            ;

        [TestMethod]
        public void GetWindowsSyncFilterTest()
        {
            var windows = GetTestForms();

            TraceFile
                .Get("GetWindowsSyncFilterTest")
                .Each(
                    windows,
                    (trace, window) => trace.Out
                        (
                            "{0}:[{1}] ({2}) [{3}]",
                            window.Canonical,
                            window.Title,
                            window.Class,
                            window.OsVisibility
                        )
                )
                .Close()
            ;
        }

        [TestMethod]
        public void HideSyncTest()
        {
            var window = GetTestForms().First();
            Assert.IsNotNull(WindowOsService.HideSync(window));
            WindowOsService.UnmanageSync(window);
        }

        [TestMethod]
        public void ShowSyncTest()
        {
            var window = GetTestForms().First();
            Assert.IsNotNull(WindowOsService.ShowSync(window));
            WindowOsService.UnmanageSync(window);
        }

        [TestMethod]
        public void MoveSyncTest()
        {
            var window = GetTestForms().First();
            Assert.IsNotNull(WindowOsService.MoveSync(window, new Rectangle(800, 600, 2000, 200)));
            WindowOsService.FocusWindowSync(window);
            Thread.Sleep(5000);
            WindowOsService.UnmanageSync(window);
        }
    }
}