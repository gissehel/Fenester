using Fenester.Lib.Business.Domain.Fenester;
using Fenester.Lib.Core.Domain.Key;
using Fenester.Lib.Core.Enums;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Test.Tools.Win;
using Fenester.Lib.Win.Enums;
using Fenester.Lib.Win.Service;
using Fenester.Lib.Win.Service.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orissev.Win32;
using Orissev.Win32.Enums;
using System;
using System.Linq;

namespace Fenester.Lib.Win.Test
{
    [TestClass]
    public abstract class KeyServiceTestBase<T> : DebuggableTest<T, IKeyService> where T : IKeyService, ITracable
    {
        public RunService RunServiceImpl { get; set; }
        public IRunServiceWin RunServiceWin => RunServiceImpl;
        public IRunService RunService => RunServiceImpl;

        protected override void InitTracesPost()
        {
            Win32Window.Tracable = this;
        }

        protected override void UninitTracesPre()
        {
            Win32Window.Tracable = null;
        }

        protected abstract T CreateMainService();

        protected override void CreateComponents()
        {
            RunServiceImpl = new RunService(RunWindowStrategy.WinForms);
            ServiceImpl = CreateMainService();

            AddComponent(RunService);
            AddComponent(Service);
        }

        public IKey GetKey(string name) => Service.GetKeys().Where(k => k.Name == name).FirstOrDefault();

        [TestMethod]
        public void RegisterShortcutTest()
        {
            TraceFile.SetName(string.Format("{0}-RegisterShortcutTest", GetType().Name));
            RunServiceWin.AddFuncMessageProcessor((message) =>
            {
                this.LogLine("  Message : {0} - {1} - {2} - {3}", message.handle.ToRepr(), message.message.ToRepr(), message.wParam.ToString(), message.lParam.ToString());
                return IntPtr.Zero;
            });
            int count = 0;
            var shortcut = Service.GetShortcut(GetKey("N"), KeyModifier.Alt);
            var operation = new Operation("Test", () =>
            {
                this.LogLine(string.Format("  Shortcut called"));
                count++;
            });
            this.LogLine("Start main call");
            var registeredShortcut = Service.RegisterShortcut(shortcut, operation);
            this.LogLine("Stop main call");

            Win32.PostMessage(RunServiceWin.Handle, WM.USER + 2, 0, 0);
            Win32.PostMessage(RunServiceWin.Handle, WM.USER + 5, 0, 0);
            Win32.PostMessage(RunServiceWin.Handle, WM.USER + 7, 0, 0);
            RunService.RunFor(new TimeSpan(0, 0, 10));
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void RegisterShortcutTestNoTimeout()
        {
            TraceFile.SetName(string.Format("{0}-RegisterShortcutTestNoTimeout", GetType().Name));
            int count = 0;
            var shortcutN = Service.GetShortcut(GetKey("N"), KeyModifier.Alt);
            var shortcutS = Service.GetShortcut(GetKey("S"), KeyModifier.Alt);
            var operation = new Operation("Test", () =>
            {
                this.LogLine(string.Format("  Shortcut called"));
                count++;
            });
            this.LogLine("Start main call");
            var registeredShortcutN = Service.RegisterShortcut(shortcutN, operation);
            var registeredShortcutS = Service.RegisterShortcut(shortcutS, new Operation("Quit", () => { RunService.Stop(); }));
            this.LogLine("Stop main call");

            RunService.Run();
            Assert.AreEqual(1, count);
        }
    }
}