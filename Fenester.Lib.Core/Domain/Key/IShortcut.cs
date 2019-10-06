using Fenester.Lib.Core.Enums;

namespace Fenester.Lib.Core.Domain.Key
{
    public interface IShortcut
    {
        string Name { get; }

        IKey Key { get; }

        KeyModifier KeyModifier { get; }

        bool Ctrl { get; set; }

        bool Win { get; set; }

        bool Alt { get; set; }

        bool Shift { get; set; }
    }
}