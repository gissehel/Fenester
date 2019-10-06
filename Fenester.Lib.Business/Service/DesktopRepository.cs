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

        public DesktopRepository()
        {
            Desktops = new List<IDesktop>();
            for (int index = 1; index <= 10; index++)
            {
                Desktops.Add(new Desktop(index));
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

        public void Init()
        {
        }

        public void Uninit()
        {
        }
    }
}