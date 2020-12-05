using Fenester.Lib.Core.Domain.Os;
using System.Collections.Generic;

namespace Fenester.Lib.Core.Service
{
    public interface IScreenOsService : IComponent
    {
        IEnumerable<IInternalScreen> GetScreens();
    }
}