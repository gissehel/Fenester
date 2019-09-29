using Fenester.Lib.Core.Domain.Utils;

namespace Fenester.Lib.Core.Domain.Graphical

{
    public interface IPosition : ICanon
    {
        int Left { get; set; }

        int Top { get; set; }

        IPosition Clone();
    }
}