using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Utils;
using Fenester.Lib.Core.Enums;

namespace Fenester.Lib.Core.Domain.Os
{
    public interface IWindow : ICanon
    {
        IWindowId Id { get; }

        string Title { get; }

        string Class { get; }

        IRectangle Rectangle { get; }

        IRectangle RectangleCurrent { get; }

        Visibility OsVisibility { get; }

        WindowCategory Category { get; }

        IWindow Clone();
    }
}