using Fenester.Lib.Core.Domain.Fenester;

namespace Fenester.Lib.Core.Domain.Key
{
    public interface IRegisteredShortcut
    {
        IShortcut Shortcut { get; }

        IOperation Operation { get; }
    }
}