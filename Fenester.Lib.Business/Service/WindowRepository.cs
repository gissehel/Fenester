using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fenester.Lib.Business.Service
{
    public class WindowRepository : IWindowRepository
    {
        private IEqualityComparer<IWindowId> WindowIdEqualityComparer { get; }

        private IDictionary<IWindowId, IInternalWindow> WindowsById { get; }

        public WindowRepository(IEqualityComparer<IWindowId> windowIdEqualityComparer)
        {
            WindowIdEqualityComparer = windowIdEqualityComparer;
            WindowsById = GetWindowDictionary<IInternalWindow>();
        }

        private void Update(IInternalWindow windowToUpdate, IWindow window)
        {
            windowToUpdate.UpdateFrom(window);
        }

        public Task AddOrUpdateWindow(IInternalWindow window)
        {
            if (WindowsById.ContainsKey(window.Id))
            {
                Update(WindowsById[window.Id], window);
            }
            else
            {
                WindowsById[window.Id] = window.Clone();
            }
            return Task.CompletedTask;
        }

        public Task<IWindow> GetWindow(IWindowId id)
        {
            IWindow result = null;
            if (WindowsById.ContainsKey(id))
            {
                result = WindowsById[id];
            }

            return Task.FromResult(result);
        }

        public Task<IEnumerable<IWindow>> GetWindows() => Task.FromResult<IEnumerable<IWindow>>(WindowsById.Values);

        public Task<bool> HasWindow(IWindowId id) => Task.FromResult(WindowsById.ContainsKey(id));

        public bool Equals(IWindowId windowId1, IWindowId windowId2) => WindowIdEqualityComparer.Equals(windowId1, windowId2);

        public IDictionary<IWindowId, T> GetWindowDictionary<T>() => new Dictionary<IWindowId, T>(WindowIdEqualityComparer);

        public void Init()
        {
        }

        public void Uninit()
        {
        }
    }
}