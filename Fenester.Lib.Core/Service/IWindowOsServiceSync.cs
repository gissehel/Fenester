using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Os;
using System.Collections.Generic;

namespace Fenester.Lib.Core.Service
{
    public interface IWindowOsServiceSync : IInitializable
    {
        IEnumerable<IInternalWindow> GetWindowsSync();

        IWindow UpdateCurrentPositionSync(IWindow iWindow);

        IWindow ShowSync(IWindow iWindow);

        IWindow HideSync(IWindow iWindow);

        IWindow MoveSync(IWindow iWindow, IRectangle rectangle);

        IWindow FocusWindowSync(IWindow iWindow);

        IWindow UnmanageSync(IWindow iWindow);
    }
}