using Fenester.Lib.Core.Domain.Fenester;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fenester.Lib.Core.Service
{
    public interface IDesktopRepository : IInitializable
    {
        Task<IEnumerable<IDesktop>> GetDesktops();

        Task<IDesktop> GetPrev(IDesktop desktop);

        Task<IDesktop> GetNext(IDesktop desktop);

        Task<IDesktop> GetById(string id);
    }
}