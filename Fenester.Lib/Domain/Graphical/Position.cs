using Fenester.Lib.Core.Domain.Graphical;

namespace Fenester.Lib.Graphical.Domain.Graphical
{
    public class Position : IPosition
    {
        private IVector Vector { get; set; }

        public Position(int x, int y)
        {
            Vector = new Vector(x, y);
        }

        public Position(IVector vector)
        {
            Vector = vector.Clone();
        }

        public int Left
        {
            get => Vector.X;
            set => Vector.X = value;
        }

        public int Top
        {
            get => Vector.Y;
            set => Vector.Y = value;
        }

        public string Canonical => string.Format("{0}{1}{2}{3}", Left < 0 ? "" : "+", Left, Top < 0 ? "" : "+", Top);

        public IPosition Clone() => new Position(Left, Top);
    }
}