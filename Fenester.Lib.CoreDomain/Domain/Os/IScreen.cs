using Fenester.Lib.Core.Domain.Graphical;

namespace Fenester.Lib.Core.Domain.Os
{
    public interface IScreen
    {
        IRectangle Rectangle { get; }

        string Name { get; }

        int Index { get; }
    }
}