using Fenester.Lib.Core.Domain.Graphical;

namespace Fenester.Lib.Graphical.Domain.Graphical
{
    public class Size : ISize
    {
        private IVector Vector { get; set; }

        public Size(IVector vector)
        {
            Vector = vector.Clone();
        }

        public Size(int width, int height)
        {
            Vector = new Vector(width, height);
        }

        public int Width
        {
            get => Vector.X;
            set => Vector.X = value;
        }

        public int Height
        {
            get => Vector.Y;
            set => Vector.Y = value;
        }

        public string Canonical => string.Format("{0}x{1}", Width, Height);

        public ISize Clone()
        {
            return new Size(Width, Height);
        }
    }
}