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
    public class KeyServiceTest : DebuggableTest<KeyService, IKeyService>
    {
        private RunService RunServiceImpl { get; set; }
        public IRunServiceWin RunServiceWin => RunServiceImpl;
        private IRunService RunService => RunServiceImpl;

        protected override void CreateServices()
        {
            RunServiceImpl = new RunService(RunWindowStrategy.WinForms);
            ServiceImpl = new KeyService(RunServiceImpl);
        }

        protected override InitializableExpressions GetInitializableExpressions
            => new InitializableExpressions
            {
                () => RunServiceImpl,
                () => ServiceImpl,
            };

        protected override void InitTraces()
        {
            Win32Window.Tracable = this;
        }

        protected override void UninitTraces()
        {
            Win32Window.Tracable = null;
        }

        protected override IEnumerable<ITracable> Tracables => new List<ITracable> { RunServiceImpl, ServiceImpl };

        public IKey GetTestKey(string name) => Service.GetKeys().Where(k => k.Name == name).FirstOrDefault();

        [TestMethod]
        public void RegisterShortcutTest()
        {
            TraceFile.SetName("RegisterShortcutTest");
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

            RunService.RunFor(new TimeSpan(0, 0, 10));
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void RegisterShortcutTestNoTimeout()
        {
            TraceFile.SetName("RegisterShortcutTest");
            int count = 0;
            var shortcutN = Service.GetShortcut(GetTestKey("N"), KeyModifier.Alt);
            var shortcutS = Service.GetShortcut(GetTestKey("S"), KeyModifier.Alt);
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