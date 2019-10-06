using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Graphical.Domain.Graphical;
using System;

namespace Fenester.Lib.Graphical.Service
{
    public class RectangleService : IRectangleService, ITracable, IInitializable
    {
        public Action<string> OnLogLine { get; set; }

        public IRectangle Intersect(IRectangle rectangle1, IRectangle rectangle2)
        {
            if (rectangle1 == null || rectangle2 == null)
            {
                return null;
            }

            var maxLeft = Math.Max(rectangle1.Left(), rectangle2.Left());
            var minRight = Math.Min(rectangle1.Right(), rectangle2.Right());

            var maxTop = Math.Max(rectangle1.Top(), rectangle2.Top());
            var minBottom = Math.Min(rectangle1.Bottom(), rectangle2.Bottom());

            if (maxLeft < minRight && maxTop < minBottom)
            {
                return new Rectangle(minRight - maxLeft, minBottom - maxTop, maxLeft, maxTop);
            }
            return null;
        }

        public void Init()
        {
        }

        public void Uninit()
        {
        }
    }
}