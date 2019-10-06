using Fenester.Lib.Core.Domain.Utils;

namespace Fenester.Lib.Core.Domain.Graphical
{
    public interface IRectangle : ICanon
    {
        IPosition Position { get; }

        ISize Size { get; }

        IRectangle Clone();
    }

    public static class IRectangleExtension
    {
        public static int Left(this IRectangle rectangle) => rectangle.Position.Left;

        public static int Right(this IRectangle rectangle) => rectangle.Position.Left + rectangle.Size.Width;

        public static int Top(this IRectangle rectangle) => rectangle.Position.Top;

        public static int Bottom(this IRectangle rectangle) => rectangle.Position.Top + rectangle.Size.Height;

        public static int Width(this IRectangle rectangle) => rectangle.Size.Width;

        public static int Height(this IRectangle rectangle) => rectangle.Size.Height;
    }
}