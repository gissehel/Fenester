using Fenester.Lib.Core.Domain.Os;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fenester.Lib.Core.Service
{
    public interface IScreenRepository
    {
        Task<IEnumerable<IScreen>> GetScreens();

        Task AddOrUpdateScreen(IInternalScreen internalScreen);

        Task<IScreen> GetPrev(IScreen screen);

        Task<IScreen> GetNext(IScreen screen);

        Task ChangeOrder(IEnumerable<IScreen> screens);
    }
}