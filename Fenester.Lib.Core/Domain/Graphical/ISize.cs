using Fenester.Lib.Core.Domain.Utils;

namespace Fenester.Lib.Core.Domain.Graphical
{
    public interface ISize : ICanon
    {
        int Width { get; set; }

        int Height { get; set; }

        ISize Clone();
    }
}