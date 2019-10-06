using Fenester.Lib.Core.Domain.Graphical;

namespace Fenester.Lib.Graphical.Domain.Graphical
{
    internal class Vector : IVector
    {
        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public string Canonical => string.Format("({0},{1})", X, Y);

        public IVector Clone() => new Vector(X, Y);
    }
}