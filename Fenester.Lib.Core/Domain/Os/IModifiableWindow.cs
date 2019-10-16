using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Enums;

namespace Fenester.Lib.Core.Domain.Os
{
    public interface IModifiableWindow : IWindow
    {
        string Title { get; set; }

        string Class { get; set; }

        IRectangle Rectangle { get; set; }

        IRectangle RectangleCurrent { get; set; }

        Visibility OsVisibility { get; set; }

        WindowCategory Category { get; set; }

        void UpdateFrom(IWindow window);
    }
}