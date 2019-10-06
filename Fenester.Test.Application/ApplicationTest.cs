using Fenester.Lib.Business.Domain.Fenester;
using Fenester.Lib.Business.Service;
using Fenester.Lib.Core.Domain.Key;
using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Enums;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Graphical.Domain.Graphical;
using Fenester.Lib.Graphical.Service;
using Fenester.Lib.Test.Tools.Win;
using Fenester.Lib.Win.Domain.Os;
using Fenester.Lib.Win.Service;
using Fenester.Lib.Win.Service.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fenester.Test.Application
{
    [TestClass]
    public class ApplicationTest : DebuggableTest
    {
        #region DebuggableTest overrides

        protected override IEnumerable<ITracable> Tracables => base.Tracables;

        protected override void CreateServices()
        {
            RunServiceImpl = new RunService();
            KeyServiceImpl = new KeyService(RunServiceImpl);
            RectangleServiceImpl = new RectangleService();
            WindowOsServiceImpl = new WindowOsService();
            WindowRepositoryImpl = new WindowRepository(new WindowId.WindowIdEqualityComparer());
            ScreenOsServiceImpl = new ScreenOsService();
            ScreenRepositoryImpl = new ScreenRepository();
        }

        protected override IEnumerable<Expression<Func<IInitializable>>> GetInitializaleExpressions => new List<Expression<Func<IInitializable>>>()
        {
            ()=>RunServiceImpl,
            ()=>KeyServiceImpl,
            ()=>RectangleServiceImpl,
            ()=>WindowOsServiceImpl,
            ()=>WindowRepositoryImpl,
            ()=>ScreenOsServiceImpl,
            ()=>ScreenRepositoryImpl,
        };

        protected override void InitTraces()
        {
            Win32Window.Tracable = this;
            Win32Monitor.Tracable = this;
        }

        protected override void UninitTraces()
        {
            Win32Monitor.Tracable = this;
            Win32Window.Tracable = this;
        }

        #endregion DebuggableTest overrides

        #region Services

        public RunService RunServiceImpl { get; set; }
        public IRunService RunService => RunServiceImpl;

        public KeyService KeyServiceImpl { get; set; }
        public IKeyService KeyService => KeyServiceImpl;

        public WindowOsService WindowOsServiceImpl { get; set; }
        public IWindowOsService WindowOsService => WindowOsServiceImpl;

        public WindowRepository WindowRepositoryImpl { get; set; }
        public IWindowRepository WindowRepository => WindowRepositoryImpl;

        public ScreenOsService ScreenOsServiceImpl { get; set; }
        public IScreenOsService ScreenOsService => ScreenOsServiceImpl;

        public ScreenRepository ScreenRepositoryImpl { get; set; }
        public IScreenRepository ScreenRepository => ScreenRepositoryImpl;

        public RectangleService RectangleServiceImpl { get; set; }
        public IRectangleService RectangleService => RectangleServiceImpl;

        #endregion Services

        public IKey GetTestKey(string name) => KeyService
            .GetKeys()
            .Where(k => k.Name == name)
            .FirstOrDefault();

        private class WindowMask
        {
            public string Class { get; set; }
            public string Title { get; set; }
        }

        private bool CheckMask(WindowMask mask, IWindow window)
        {
            if (mask.Class != null && mask.Title == null)
            {
                return window.Class == mask.Class;
            }
            if (mask.Class != null && mask.Title != null)
            {
                return window.Class == mask.Class && window.Title == mask.Title;
            }
            if (mask.Class == null && mask.Title != null)
            {
                return window.Title == mask.Title;
            }
            return false;
        }

        private List<WindowMask> TestMasks { get; set; } = new List<WindowMask>
        {
            new WindowMask { Class = "TTOTAL_CMD" },
            new WindowMask { Class = "TFormMain" },
            new WindowMask { Class = "TLister" },
            new WindowMask { Class = "ApplicationFrameWindow", Title="Netflix" },
        };

        public IEnumerable<IWindow> GetTestWindows => WindowOsServiceImpl
            .GetWindowsSync()
            .Cast<IWindow>()
            .Where(w => TestMasks.Any(testMask => CheckMask(testMask, w)))
            ;

        public IEnumerable<IWindow> GetTrayWindows => WindowOsServiceImpl
            .GetWindowsSync()
            .Cast<IWindow>()
            .Where(w => w.Class.StartsWith("Shell_") && w.Class.EndsWith("TrayWnd"))
            ;

        private void AddAction(KeyModifier keyModifier, string keyName, string operationName, Action action)
        {
            var shortcut = KeyService.GetShortcut(GetTestKey(keyName), keyModifier);
            var operation = new Operation(operationName, action);
            KeyService.RegisterShortcut(shortcut, operation);
        }

        private void DispatchOnScreen(List<IWindow> windows, IScreen screen)
        {
            var windowsCount = windows.Count;

            if (windowsCount > 0)
            {
                var totalWidth = screen.Rectangle.Size.Width;
                var width = (totalWidth - (windowsCount - 1) * 10) / windowsCount;

                for (int windowIndex = 0; windowIndex < windowsCount; windowIndex++)
                {
                    var left = screen.Rectangle.Position.Left + (width + 10) * windowIndex;
                    var top = screen.Rectangle.Position.Top;
                    var height = screen.Rectangle.Size.Height;
                    var rectangle = new Rectangle(width, height, left, top);
                    var window = windows[windowIndex];
                    WindowOsServiceImpl.MoveSync(window, rectangle);

                    this.LogLine("Moving window {0} to {1}", window.ToRepr(), rectangle.Canonical);
                }
            }
        }

        [TestMethod]
        public void MultiFocusTest()
        {
            TraceFile.SetName("MultiFocusTest");

            AddAction(KeyModifier.Alt, "S", "Quit", () => RunService.Stop());

            var testWindows = GetTestWindows.ToList();
            var testWindowsCount = testWindows.Count;
            int currentPosition = -1;

            var screens = ScreenOsService.GetScreens();
            IScreen screen = screens.Last();

            foreach (var testWindow in testWindows)
            {
                this.LogLine("Window : {0}", testWindow.ToRepr());
            }

            var trayWindows = GetTrayWindows;
            trayWindows = trayWindows.Where(w => RectangleService.Intersect(w.RectangleCurrent, screen.Rectangle) != null);

            foreach (var trayWindowItem in trayWindows)
            {
                this.LogLine("Tray Window : {0}", trayWindowItem.ToRepr());
            }
            var trayWindow = trayWindows.First();

            void focusNext()
            {
                currentPosition++;
                currentPosition %= testWindowsCount;
                this.LogLine("  -> Focusing {0}]", testWindows[currentPosition].ToRepr());
                WindowOsServiceImpl.FocusWindowSync(testWindows[currentPosition]);
            }

            void focusPrev()
            {
                currentPosition += testWindowsCount;
                currentPosition--;
                currentPosition %= testWindowsCount;
                this.LogLine("  -> Focusing {0}]", testWindows[currentPosition].ToRepr());
                WindowOsServiceImpl.FocusWindowSync(testWindows[currentPosition]);
            }

            void moveToNewPosition(int newPosition)
            {
                var currentWindow = testWindows[currentPosition];
                testWindows.Remove(currentWindow);
                testWindows.Insert(newPosition, currentWindow);

                this.LogLine("  -> Moving {0}]", currentWindow.ToRepr());

                DispatchOnScreen(testWindows, screen);
                currentPosition = newPosition;
                WindowOsServiceImpl.FocusWindowSync(currentWindow);
            }

            void moveToNext()
            {
                var newPosition = (currentPosition + 1) % testWindowsCount;
                moveToNewPosition(newPosition);
            }

            void moveToPrev()
            {
                var newPosition = (currentPosition + testWindowsCount - 1) % testWindowsCount;
                moveToNewPosition(newPosition);
            }

            AddAction(KeyModifier.Alt, "Left", "FocusPrev", focusPrev);
            AddAction(KeyModifier.Alt, "Right", "FocusNext", focusNext);
            AddAction(KeyModifier.Alt, "J", "MoveToPrev", moveToPrev);
            AddAction(KeyModifier.Alt, "K", "MoveToNext", moveToNext);

            DispatchOnScreen(testWindows, screen);

            WindowOsServiceImpl.HideSync(trayWindow);
            focusNext();
            RunService.Run();

            foreach (var testWindow in testWindows)
            {
                WindowOsServiceImpl.UnmanageSync(testWindow);
            }
            WindowOsServiceImpl.ShowSync(trayWindow);
        }
    }
}