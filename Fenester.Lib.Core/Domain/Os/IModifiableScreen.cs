using Fenester.Lib.Core.Domain.Graphical;

namespace Fenester.Lib.Core.Domain.Os
{
    public interface IModifiableScreen
    {
        IRectangle Rectangle { get; set; }

        int Index { get; set; }

        void UpdateFrom(IScreen window);
    }
}