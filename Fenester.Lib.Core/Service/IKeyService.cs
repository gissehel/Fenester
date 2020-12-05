using Fenester.Lib.Core.Domain.Fenester;
using Fenester.Lib.Core.Domain.Key;
using Fenester.Lib.Core.Enums;
using System.Collections.Generic;

namespace Fenester.Lib.Core.Service
{
    public interface IKeyService : IComponent
    {
        IEnumerable<IKey> GetKeys();

        IShortcut GetShortcut(IKey iKey, KeyModifier keyModifier);

        IRegisteredShortcut RegisterShortcut(IShortcut shortcut, IOperation operation);

        void UnregisterShortcut(IRegisteredShortcut registeredShortcut);
    }
}