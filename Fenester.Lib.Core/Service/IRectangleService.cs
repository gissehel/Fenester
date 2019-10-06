using Fenester.Lib.Core.Domain.Graphical;

namespace Fenester.Lib.Core.Service
{
    public interface IRectangleService
    {
        IRectangle Intersect(IRectangle rectangle1, IRectangle rectangle2);
    }
}