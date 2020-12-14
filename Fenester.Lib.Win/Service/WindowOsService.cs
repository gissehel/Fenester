using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Win.Domain.Os;
using Fenester.Lib.Win.Service.Helpers;
using Orissev.Win32;
using Orissev.Win32.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fenester.Lib.Win.Service
{
    public class WindowOsService : IWindowOsService, IWindowOsServiceSync, ITracable
    {
        public Action<string> OnLogLine { get; set; }

        public WindowOsService()
        {
        }

        #region Async

        private async Task<T> RunAsync<T>(Func<T> func)
        {
            await Task.Delay(0);
            return func();
            // return Task.Run<T>(() => func());
        }

        public Task<IWindow> UpdateCurrentPosition(IWindow window) => RunAsync(() => UpdateCurrentPositionSync(window));

        public Task<IEnumerable<IInternalWindow>> GetWindows() => RunAsync(() => GetWindowsSync());

        public Task<IWindow> Show(IWindow window) => RunAsync(() => ShowSync(window));

        public Task<IWindow> Hide(IWindow window) => RunAsync(() => HideSync(window));

        public Task<IWindow> Move(IWindow iWindow, IRectangle rectangle) => RunAsync(() => MoveSync(iWindow, rectangle));

        public Task<IWindow> FocusWindow(IWindow iWindow) => RunAsync(() => FocusWindowSync(iWindow));

        public Task<IWindow> Unmanage(IWindow iWindow) => RunAsync(() => UnmanageSync(iWindow));

        #endregion Async

        #region private

        private void EnsureWindowInitialStyles(Window window)
        {
            if (window.InitialWindowProps == null)
            {
                if (Win32Window.GetWindowStyles(window.Handle, out WS styles, out WS_EX extStyles))
                {
                    window.InitialWindowProps = new InitialWindowProps()
                    {
                        RectangleCurrent = window.RectangleCurrent?.Clone(),
                        WinStyles = styles,
                        WinExStyles = extStyles,
                    };
                    Win32Window.ChangeWindowStyles(window.Handle, 0, WS.MINIMIZEBOX | WS.MAXIMIZEBOX | WS.THICKFRAME, 0, 0);
                }
            }
        }

        private void EnsureWindowInitialStyles(IWindow iWindow)
        {
            if (iWindow is Window window)
            {
                EnsureWindowInitialStyles(window);
            }
        }

        #endregion private

        #region Sync

        public IWindow UpdateCurrentPositionSync(IWindow iWindow)
        {
            if (iWindow is Window window)
            {
                EnsureWindowInitialStyles(window);
                if (Win32Window.GetWindowProps(window))
                {
                    return window;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public IEnumerable<IInternalWindow> GetWindowsSync()
        {
            return Win32Window.GetOpenWindows().Values.OrderBy((window) => window.Handle.ToInt64());
        }

        public IWindow ShowSync(IWindow iWindow)
        {
            if (iWindow is Window window)
            {
                EnsureWindowInitialStyles(window);
                Win32.ShowWindow(window.Handle, SW.SHOW);
                if (Win32Window.GetWindowProps(window))
                {
                    return window;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public IWindow HideSync(IWindow iWindow)
        {
            if (iWindow is Window window)
            {
                EnsureWindowInitialStyles(window);
                Win32.ShowWindow(window.Handle, SW.HIDE);
                if (Win32Window.GetWindowProps(window))
                {
                    return window;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public IWindow MoveSync(IWindow iWindow, IRectangle rectangle)
        {
            if (iWindow is Window window)
            {
                EnsureWindowInitialStyles(window);
                Win32Window.MoveWindowAndRedraw(window.Id.Handle, rectangle);
                if (Win32Window.GetWindowProps(window))
                {
                    return window;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public IWindow FocusWindowSync(IWindow iWindow)
        {
            if (iWindow is Window window)
            {
                EnsureWindowInitialStyles(window);
                Win32Window.FocusWindow2(window.Handle);
                if (Win32Window.GetWindowProps(window))
                {
                    return window;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public IWindow UnmanageSync(IWindow iWindow)
        {
            if (iWindow is Window window)
            {
                if (window.InitialWindowProps == null)
                {
                }
                else
                {
                    Win32Window.SetWindowStyles(window.Id.Handle, window.InitialWindowProps.WinStyles, window.InitialWindowProps.WinExStyles);
                    if (window.InitialWindowProps.RectangleCurrent != null)
                    {
                        Win32Window.MoveWindowAndRedraw(window.Id.Handle, window.InitialWindowProps.RectangleCurrent);
                    }
                }
                if (Win32Window.GetWindowProps(window))
                {
                    return window;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        #endregion Sync

        public void Init()
        {
        }

        public void Uninit()
        {
        }
    }
}