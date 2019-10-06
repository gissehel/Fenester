using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Enums;

namespace Fenester.Lib.Core.Domain.Fenester
{
    public interface IFenesterWindow
    {
        IWindowId Id { get; }

        IWindow Window { get; set; }

        Visibility FenesterVisibility { get; set; }

        bool Floating { get; set; }

        IDesktop Desktop { get; set; }
    }
}