using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Service;
using Fenester.Test.Mock.Domain.Os;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fenester.Test.Mock.Service
{
    public class WindowOsServiceMock : IWindowOsServiceSync, IWindowOsServiceSyncMock
    {
        public Func<IWindow, IWindow> OnFocusWindowSync { get; set; }

        public Func<IEnumerable<IInternalWindow>> OnGetWindowsSync { get; set; }

        public Func<IWindow, IWindow> OnHideSync { get; set; }

        public Func<IWindow, IWindow> OnShowSync { get; set; }

        public Func<IWindow, IRectangle, IWindow> OnMoveSync { get; set; }

        public Func<IWindow, IWindow> OnUpdateCurrentPositionSync { get; set; }

        public Func<IWindow, IWindow> OnUnmanageSync { get; set; }

        public Action OnInit { get; set; }

        public Action OnUninit { get; set; }

        public void Init()
        {
            OnInit?.Invoke();
        }

        public void Uninit()
        {
            OnUninit?.Invoke();
        }

        public Dictionary<string, WindowMock> Windows { get; } = new Dictionary<string, WindowMock>();
        public WindowMock FocusedWindow { get; set; }

        public IWindow FocusWindowSync(IWindow iWindow)
        {
            if (OnFocusWindowSync != null)
            {
                return OnFocusWindowSync(iWindow);
            }
            else
            {
                var window = FindOwnWindow(iWindow);
                if (window != null)
                {
                    FocusedWindow = window;
                    return FocusedWindow.Clone();
                }
                return null;
            }
        }

        public IEnumerable<IInternalWindow> GetWindowsSync()
        {
            if (OnGetWindowsSync != null)
            {
                return OnGetWindowsSync();
            }
            else
            {
                return Windows.Select(x => x.Value).Select(w => w.Clone());
            }
        }

        private WindowMock FindOwnWindow(IWindow iWindow)
        {
            if (iWindow is WindowMock window)
            {
                var rawId = window.Id.RawId;
                if (Windows.ContainsKey(rawId))
                {
                    return Windows[rawId];
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
            if (OnHideSync != null)
            {
                return OnHideSync(iWindow);
            }
            else
            {
                var window = FindOwnWindow(iWindow);
                if (window != null)
                {
                    window.OsVisibility = Lib.Core.Enums.Visibility.Minimized;
                    return window.Clone();
                }
                return null;
            }
        }

        public IWindow MoveSync(IWindow iWindow, IRectangle rectangle)
        {
            if (OnMoveSync != null)
            {
                return OnMoveSync(iWindow, rectangle);
            }
            else
            {
                var window = FindOwnWindow(iWindow);
                if (window != null)
                {
                    window.RectangleCurrent = rectangle.Clone();
                    return window.Clone();
                }
                return null;
            }
        }

        public IWindow ShowSync(IWindow iWindow)
        {
            if (OnShowSync != null)
            {
                return OnShowSync(iWindow);
            }
            else
            {
                var window = FindOwnWindow(iWindow);
                if (window != null)
                {
                    window.OsVisibility = Lib.Core.Enums.Visibility.Normal;
                    return window.Clone();
                }
                return null;
            }
        }

        public IWindow UnmanageSync(IWindow iWindow)
        {
            if (OnUnmanageSync != null)
            {
                return OnUnmanageSync(iWindow);
            }
            else
            {
                var window = FindOwnWindow(iWindow);
                if (window != null)
                {
                    return window.Clone();
                }
                return null;
            }
        }

        public IWindow UpdateCurrentPositionSync(IWindow iWindow)
        {
            if (OnUpdateCurrentPositionSync != null)
            {
                return UpdateCurrentPositionSync(iWindow);
            }
            else
            {
                var window = FindOwnWindow(iWindow);
                if (window != null)
                {
                    return window.Clone();
                }
                return null;
            }
        }

        public void AddWindow(WindowMock window)
        {
            Windows[window.Id.RawId] = window;
        }
    }
}