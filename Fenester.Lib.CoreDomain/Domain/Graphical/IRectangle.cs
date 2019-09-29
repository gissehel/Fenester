using Fenester.Lib.Core.Domain.Utils;

namespace Fenester.Lib.Core.Domain.Graphical
{
    public interface IRectangle : ICanon
    {
        IPosition Position { get; }

        ISize Size { get; }

        IRectangle Clone();
    }
}