using Fenester.Lib.Business.Domain.Fenester;
using Fenester.Lib.Core.Domain.Key;
using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Domain.Utils;
using Fenester.Lib.Core.Enums;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Graphical.Domain.Graphical;
using Fenester.Lib.Test.Tools.Win;
using Fenester.Lib.Win.Enums;
using Fenester.Lib.Win.Service;
using Fenester.Lib.Win.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fenester.Exe.InteractiveTest
{
    public class Application : ComponentManager
    {
        private TraceFile TraceFile { get; set; }

        private ImplementedProperty<IKeyService, KeyServiceRawInput> KeyService { get; } = new ImplementedProperty<IKeyService, KeyServiceRawInput>();

        private ImplementedProperty<IRunServiceWin, RunService> RunService { get; } = new ImplementedProperty<IRunServiceWin, RunService>();

        private ImplementedProperty<IWindowOsService, WindowOsService> WindowOsService { get; } = new ImplementedProperty<IWindowOsService, WindowOsService>();

        private ImplementedProperty<IScreenOsService, ScreenOsService> ScreenOsService { get; } = new ImplementedProperty<IScreenOsService, ScreenOsService>();

        protected override void CreateTraces()
        {
            base.CreateTraces();
            TraceFile = new TraceFile();
            OnLogLine = (line) => TraceFile.OutLine(line);
            Tracable.DefaultOnLogLine = OnLogLine;
        }

        protected override void DisposeTraces()
        {
            TraceFile.Close();
            TraceFile = null;
            OnLogLine = null;
            Tracable.DefaultOnLogLine = null;
        }

        protected override void CreateComponents()
        {
            base.CreateComponents();
            RunService.Impl = new RunService(RunWindowStrategy.Win32);
            KeyService.Impl = new KeyServiceRawInput(RunService.Use);
            WindowOsService.Impl = new WindowOsService();
            ScreenOsService.Impl = new ScreenOsService();
            // Win32Window.Tracable = this; 

            AddComponent(RunService.Use);
            AddComponent(KeyService.Use);
            AddComponent(WindowOsService.Use);
            AddComponent(ScreenOsService.Use);
        }

        public IKey GetKey(string name) => KeyService.Use.GetKeys().Where(key => key.Name == name).FirstOrDefault();

        public void RegisterAction(string keyName, KeyModifier keyModifier, string name, Action action)
        {
            KeyService.Use.RegisterShortcut(KeyService.Use.GetShortcut(GetKey(keyName), keyModifier), new Operation(name, action));
        }

        private async Task<IWindow> GetWindow()
        {
            var windows = await WindowOsService.Use.GetWindows();
            var window = windows.Where(w => w?.Title != null).Where(w => w.Title.Contains("Films")).FirstOrDefault();
            return window;
        }

        protected override void InitServicesPost()
        {
            base.InitServicesPost();
            TraceFile.SetName("Fenester.Exe.InteractiveTest");
            int count = 0;

            Action testAction = () =>
            {
                this.LogLine("  Shortcut called ({0})", count);
                count++;
            };
            Action desktopAction = () =>
            {
                foreach (var screen in ScreenOsService.Use.GetScreens())
                {
                    this.LogLine("Screen [{0}][{1}] [{2}]", screen.Id, screen.Name, screen.Rectangle.Canonical);
                }
            };
            Action enumerateWindowsOperationAsync = async () =>
            {
                var windows = await WindowOsService.Use.GetWindows();
                foreach (var window in windows)
                {
                    if (window.Rectangle != null)
                    {
                        this.LogLine("    {0} ({1})", window.Title, window.Rectangle.Canonical);
                    } 
                    
                }
            };
            Action focusWindowAction = async () =>
            {
                var window = await GetWindow();
                if (window != null)
                {
                    this.LogLine("Focusing {0} ({1})", window.Title, window.Rectangle.Canonical);
                    await WindowOsService.Use.FocusWindow(window);
                }
                else
                {
                    this.LogLine("No window to focus");
                }
            };
            Action<int, int, int, int> moveWindowAction = async (int width, int height, int left, int top) =>
            {
                var window = await GetWindow();
                if (window != null)
                {
                    this.LogLine("Move+Resize {0} ({1})", window.Title, window.Rectangle.Canonical);
                    await WindowOsService.Use.Move(window, new Rectangle(width, height, left, top));
                }
                else
                {
                    this.LogLine("No window to move");
                }
            };
            this.LogLine("Start main call");
            RegisterAction("S", KeyModifier.Alt, "Quit", () => RunService.Use.Stop());
            RegisterAction("N", KeyModifier.Alt, "Test", testAction);
            RegisterAction("E", KeyModifier.Alt, "EnumerateWindows", () => enumerateWindowsOperationAsync());
            RegisterAction("D", KeyModifier.Alt, "Desktop", desktopAction);
            RegisterAction("F", KeyModifier.Alt, "Focus Window", focusWindowAction);
            RegisterAction("M", KeyModifier.Alt, "Move Window", () => moveWindowAction(500, 800, 50, 50));
            RegisterAction("P", KeyModifier.Alt, "Move Window 2", () => moveWindowAction(800, 500, 300, 500));
            this.LogLine("Stop main call");
        }

        public void Run()
        {
            RunService.Use.Run();
        }
    }
}
