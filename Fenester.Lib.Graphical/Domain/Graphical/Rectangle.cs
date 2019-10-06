using Fenester.Lib.Core.Domain.Graphical;

namespace Fenester.Lib.Graphical.Domain.Graphical
{
    public class Rectangle : IRectangle
    {
        public Rectangle(IPosition position, ISize size)
        {
            Position = position.Clone();
            Size = size.Clone();
        }

        public Rectangle(int width, int height, int left, int top)
        {
            Position = new Position(left, top);
            Size = new Size(width, height);
        }

        public IPosition Position { get; set; }

        public ISize Size { get; set; }

        public string Canonical => string.Format("{0}{1}", Size.Canonical, Position.Canonical);

        public IRectangle Clone() => new Rectangle(Size.Width, Size.Height, Position.Left, Position.Top);
    }
}