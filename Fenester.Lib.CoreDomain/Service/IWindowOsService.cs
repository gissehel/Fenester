using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Os;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fenester.Lib.Core.Service
{
    public interface IWindowOsService
    {
        Task<IEnumerable<IWindow>> GetWindows();

        Task<IRectangle> Move(IWindow window, IRectangle rectangle);

        Task<IRectangle> GetWindowCurrentPosition(IWindow window);

        Task<IWindow> Show(IWindow window);

        Task<IWindow> Hide(IWindow hide);
    }
}