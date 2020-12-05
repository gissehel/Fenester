using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Test.Tools.Win;
using Fenester.Lib.Win.Service;
using Fenester.Lib.Win.Service.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Fenester.Lib.Win.Test
{
    [TestClass]
    public class ScreenOsServiceTest : DebuggableTest<ScreenOsService, IScreenOsService>
    {
        [TestMethod]
        public void GetScreensTest()
        {
            TraceFile.SetName("GetScreensTest");

            var screens = Service.GetScreens();

            foreach (var screen in screens.Cast<IScreen>())
            {
                this.LogLine("{0} {1}", screen.Name, screen.Rectangle.ToRepr());
            }
        }

        #region DebuggableTest

        protected override void CreateComponents()
        {
            ServiceImpl = new ScreenOsService();
            AddComponent(Service);
        }

        protected override void InitTracesPost()
        {
            Win32Monitor.Tracable = this;
        }

        protected override void UninitTracesPre()
        {
            Win32Monitor.Tracable = null;
        }

        #endregion DebuggableTest
    }
}