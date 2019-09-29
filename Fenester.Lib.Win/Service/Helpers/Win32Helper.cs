using Fenester.Lib.Graphical.Domain.Graphical;
using System;

namespace Fenester.Lib.Win.Service.Helpers
{
    public static class Win32Helper
    {
        public static Rectangle GetRectangleFromRect(this Rect rect)
        {
            var width = rect.Right - rect.Left;
            var height = rect.Bottom - rect.Top;
            var left = rect.Left;
            var top = rect.Top;
            return new Rectangle(width, height, left, top);
        }

        public static int ToInt32(this IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }
    }
}