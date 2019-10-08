using Fenester.Lib.Business.Domain.Fenester;
using Fenester.Lib.Core.Domain.Key;
using Fenester.Lib.Core.Enums;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Test.Tools.Win;
using Fenester.Lib.Win.Enums;
using Fenester.Lib.Win.Service;
using Fenester.Lib.Win.Service.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fenester.Lib.Win.Test
{
    [TestClass]
    public class KeyServiceHookTest : DebuggableTest<KeyServiceHook, IKeyService>
    {
        public RunService RunServiceImpl { get; set; }
        public IRunServiceWin RunServiceWin => RunServiceImpl;
        public IRunService RunService => RunServiceImpl;

        protected override void InitTraces()
        {
            Win32Window.Tracable = this;
        }

        protected override void UninitTraces()
        {
            Win32Window.Tracable = null;
        }

        protected override void CreateServices()
        {
            RunServiceImpl = new RunService(RunWindowStrategy.WinForms);
            ServiceImpl = new KeyServiceHook();
        }

        protected override InitializableExpressions GetInitializableExpressions
            => new InitializableExpressions
            {
                () => RunServiceImpl,
                () => ServiceImpl,
            };

        protected override IEnumerable<ITracable> Tracables => new List<ITracable>
        {
            RunServiceImpl,
            ServiceImpl
        };

        public IKey GetTestKey(string name) => Service.GetKeys().Where(k => k.Name == name).FirstOrDefault();

        [TestMethod]
        public void RegisterShortcutTest()
        {
            TraceFile.SetName("RegisterShortcutTest-Hook");
            RunServiceWin.AddFuncMessageProcessor((message) =>
            {
                this.LogLine("  Message : {0} - {1} - {2} - {3}", message.handle.ToRepr(), message.message.ToRepr(), message.wParam.ToString(), message.lParam.ToString());
                return IntPtr.Zero;
            });
            int count = 0;
            var shortcut = Service.GetShortcut(GetTestKey("N"), KeyModifier.Alt);
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
            Assert.AreEqual(0, count);
        }
    }
}