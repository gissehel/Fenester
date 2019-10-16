using Fenester.Lib.Business.Domain.Fenester;
using Fenester.Lib.Core.Domain.Fenester;
using Fenester.Lib.Core.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fenester.Lib.Business.Service
{
    public class DesktopRepository : IDesktopRepository
    {
        private List<IDesktop> Desktops { get; }
        private Dictionary<string, IDesktop> DesktopsByName { get; } = new Dictionary<string, IDesktop>();

        private void Add(Desktop desktop)
        {
            Desktops.Add(desktop);
            if (DesktopsByName.ContainsKey(desktop.Id))
            {
                Desktops.Remove(DesktopsByName[desktop.Id]);
            }
            DesktopsByName[desktop.Id] = desktop;
        }

        public DesktopRepository()
        {
            Desktops = new List<IDesktop>();
            for (int index = 1; index <= 10; index++)
            {
                Add(new Desktop(index));
            }
        }

        public async Task<IEnumerable<IDesktop>> GetDesktops()
        {
            return await Task.FromResult(Desktops);
        }

        public Task<IDesktop> GetNext(IDesktop desktop)
        {
            var index = Desktops.IndexOf(desktop);
            index++;
            index = index % Desktops.Count;
            return Task.FromResult(Desktops[index]);
        }

        public Task<IDesktop> GetPrev(IDesktop desktop)
        {
            var index = Desktops.IndexOf(desktop);
            if (index < 0)
            {
                index = Desktops.Count - 1;
            }
            else
            {
                index += Desktops.Count;
                index--;
                index = index % Desktops.Count;
            }
            return Task.FromResult(Desktops[index]);
        }

        public Task<IDesktop> GetById(string id)
        {
            IDesktop desktop = null;
            if (DesktopsByName.ContainsKey(id))
            {
                desktop = DesktopsByName[id];
            }
            return Task.FromResult(desktop);
        }

        public void Init()
        {
        }

        public void Uninit()
        {
        }
    }
}