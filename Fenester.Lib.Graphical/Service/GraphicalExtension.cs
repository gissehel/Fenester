using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Graphical.Domain.Graphical;
using System;

namespace Fenester.Lib.Graphical.Service
{
    public static class GraphicalExtension
    {
        public static IVector Add(this IVector self, IVector other) => new Vector(self.X + other.X, self.Y + other.Y);

        public static IVector Sub(this IVector self, IVector other) => new Vector(self.X - other.X, self.Y - other.Y);

        public static IVector ScalarMult(this IVector self, int scalar) => new Vector(self.X * scalar, self.Y * scalar);

        public static IVector ScalarMult(this IVector self, double scalar) => new Vector(Convert.ToInt32(self.X * scalar), Convert.ToInt32(self.Y * scalar));

        public static IRectangle Move(this IRectangle self, IPosition position) => new Rectangle(position, self.Size);

        public static IRectangle Resize(this IRectangle self, ISize size) => new Rectangle(self.Position, size);

        public static IPosition ToPosition(this IVector self) => new Position(self);

        public static ISize ToSize(this IVector self) => new Size(self);
    }
}