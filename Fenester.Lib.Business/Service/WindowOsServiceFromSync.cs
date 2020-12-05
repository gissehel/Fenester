using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fenester.Lib.Business.Service
{
    public class WindowOsServiceFromSync : IWindowOsService
    {
        public Action<string> OnLogLine { get; set; }

        private IWindowOsServiceSync WindowOsServiceSync { get; }

        public WindowOsServiceFromSync(IWindowOsServiceSync windowOsServiceSync)
        {
            WindowOsServiceSync = windowOsServiceSync;
        }

        public Task<IWindow> UpdateCurrentPosition(IWindow window) => Task.Run(() => WindowOsServiceSync.UpdateCurrentPositionSync(window));

        public Task<IEnumerable<IInternalWindow>> GetWindows() => Task.Run(() => WindowOsServiceSync.GetWindowsSync());

        public Task<IWindow> Show(IWindow window) => Task.Run(() => WindowOsServiceSync.ShowSync(window));

        public Task<IWindow> Hide(IWindow window) => Task.Run(() => WindowOsServiceSync.HideSync(window));

        public Task<IWindow> Move(IWindow iWindow, IRectangle rectangle) => Task.Run(() => WindowOsServiceSync.MoveSync(iWindow, rectangle));

        public Task<IWindow> FocusWindow(IWindow iWindow) => Task.Run(() => WindowOsServiceSync.FocusWindowSync(iWindow));

        public Task<IWindow> Unmanage(IWindow iWindow) => Task.Run(() => WindowOsServiceSync.UnmanageSync(iWindow));

        public void Init()
        {
        }

        public void Uninit()
        {
        }
    }
}