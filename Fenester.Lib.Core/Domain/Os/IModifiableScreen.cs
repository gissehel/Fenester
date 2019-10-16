using Fenester.Lib.Core.Domain.Graphical;

namespace Fenester.Lib.Core.Domain.Os
{
    public interface IModifiableScreen : IScreen
    {
        new IRectangle Rectangle { get; set; }

        new int Index { get; set; }

        void UpdateFrom(IScreen screen);
    }
}