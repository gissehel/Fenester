using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Os;
using Fenester.Test.Mock.Domain.Os;
using System;
using System.Collections.Generic;

namespace Fenester.Test.Mock.Service
{
    public interface IWindowOsServiceSyncMock : IInitializableMock
    {
        Func<IWindow, IWindow> OnFocusWindowSync { get; set; }

        Func<IEnumerable<IInternalWindow>> OnGetWindowsSync { get; set; }

        Func<IWindow, IWindow> OnHideSync { get; set; }

        Func<IWindow, IWindow> OnShowSync { get; set; }

        Func<IWindow, IRectangle, IWindow> OnMoveSync { get; set; }

        Func<IWindow, IWindow> OnUpdateCurrentPositionSync { get; set; }

        Func<IWindow, IWindow> OnUnmanageSync { get; set; }

        Dictionary<string, WindowMock> Windows { get; }

        void AddWindow(WindowMock window);

        WindowMock FocusedWindow { get; set; }
    }
}