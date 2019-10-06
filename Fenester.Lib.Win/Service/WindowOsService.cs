using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Win.Domain.Os;
using Fenester.Lib.Win.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fenester.Lib.Win.Service
{
    public class WindowOsService : IWindowOsService, ITracable
    {
        public Action<string> OnLogLine { get; set; }

        public WindowOsService()
        {
        }

        #region Async

        public Task<IWindow> UpdateCurrentPosition(IWindow window) => Task.Run(() => UpdateCurrentPositionSync(window));

        public Task<IEnumerable<IWindow>> GetWindows() => Task.Run(() => GetWindowsSync());

        public Task<IWindow> Show(IWindow window) => Task.Run(() => ShowSync(window));

        public Task<IWindow> Hide(IWindow window) => Task.Run(() => HideSync(window));

        public Task<IWindow> Move(IWindow iWindow, IRectangle rectangle) => Task.Run(() => MoveSync(iWindow, rectangle));

        public Task<IWindow> FocusWindow(IWindow iWindow) => Task.Run(() => FocusWindow(iWindow));

        public Task<IWindow> Unmanage(IWindow iWindow) => Task.Run(() => UnmanageSync(iWindow));

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
            throw new NotImplementedException();
        }

        public IEnumerable<IWindow> GetWindowsSync()
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
                Win32Window.FocusWindow(window.Handle);
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