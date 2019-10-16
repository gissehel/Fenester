using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Os;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fenester.Lib.Core.Service
{
    public interface IWindowOsService : IInitializable
    {
        Task<IEnumerable<IInternalWindow>> GetWindows();

        Task<IWindow> UpdateCurrentPosition(IWindow iWindow);

        Task<IWindow> Show(IWindow iWindow);

        Task<IWindow> Hide(IWindow iWindow);

        Task<IWindow> Move(IWindow iWindow, IRectangle rectangle);

        Task<IWindow> FocusWindow(IWindow iWindow);

        Task<IWindow> Unmanage(IWindow iWindow);
    }
}