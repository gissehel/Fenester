using Fenester.Lib.Core.Domain.Os;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fenester.Lib.Core.Service
{
    public interface IWindowRepository
    {
        Task<IWindow> GetWindow(IWindowId windowId);

        Task AddOrUpdateWindow(IInternalWindow window);

        Task<bool> HasWindow(IWindowId windowId);

        Task<IEnumerable<IWindow>> GetWindows();

        bool Equals(IWindowId windowId1, IWindowId windowId2);

        IDictionary<IWindowId, T> GetWindowDictionary<T>();
    }
}