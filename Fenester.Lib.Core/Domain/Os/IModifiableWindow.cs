using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Enums;

namespace Fenester.Lib.Core.Domain.Os
{
    public interface IModifiableWindow : IWindow
    {
        new string Title { get; set; }

        new string Class { get; set; }

        new IRectangle Rectangle { get; set; }

        new IRectangle RectangleCurrent { get; set; }

        new Visibility OsVisibility { get; set; }

        new WindowCategory Category { get; set; }

        void UpdateFrom(IWindow window);
    }
}