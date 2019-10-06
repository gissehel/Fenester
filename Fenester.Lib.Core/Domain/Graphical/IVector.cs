using Fenester.Lib.Core.Domain.Utils;

namespace Fenester.Lib.Core.Domain.Graphical
{
    public interface IVector : ICanon
    {
        int X { get; set; }
        int Y { get; set; }

        IVector Clone();
    }
}