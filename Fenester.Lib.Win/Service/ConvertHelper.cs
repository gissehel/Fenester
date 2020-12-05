using Fenester.Lib.Core.Enums;
using Fenester.Lib.Graphical.Domain.Graphical;
using Fenester.Lib.Win.Enums;
using Fenester.Lib.Win.Service.Helpers;
using System;

namespace Fenester.Lib.Win.Service
{
    public static class ConvertHelper
    {
        public static KeyModifiers ToKeyModifiers(this KeyModifier keyModifier)
        {
            switch (keyModifier)
            {
                case KeyModifier.None:
                    return KeyModifiers.None;

                case KeyModifier.Ctrl:
                    return KeyModifiers.Control;

                case KeyModifier.Shift:
                    return KeyModifiers.Shift;

                case KeyModifier.Alt:
                    return KeyModifiers.Alt;

                case KeyModifier.Win:
                    return KeyModifiers.Windows;

                default:
                    return KeyModifiers.None;
            }
        }

        public static KeyModifier ToKeyModifier(this KeyModifiers keyModifiers)
        {
            switch (keyModifiers)
            {
                case KeyModifiers.None:
                    return KeyModifier.None;

                case KeyModifiers.Alt:
                    return KeyModifier.Alt;

                case KeyModifiers.Control:
                    return KeyModifier.Ctrl;

                case KeyModifiers.Shift:
                    return KeyModifier.Shift;

                case KeyModifiers.Windows:
                    return KeyModifier.Win;

                default:
                    return KeyModifier.None;
            }
        }

        public static WS ToWS(this IntPtr self) => (WS)unchecked((uint)self.ToInt32());

        public static WS_EX ToWS_EX(this IntPtr self) => (WS_EX)unchecked((uint)self.ToInt32());

        public static long ToLong<T>(this T self) where T : struct, IConvertible => Convert.ToInt64(self);

        public static string ToEnumName<T>(this T self) where T : struct, IConvertible => Enum.GetName(typeof(T), self);

        public static string ToEnumTypeName<T>(this T self) where T : struct, IConvertible => typeof(T).Name;

        public static string ToRepr<T>(this T self) where T : struct, IConvertible => string.Format("({2}:{0}/0x{0:x}) [{1}]", self.ToLong(), self.ToEnumName(), self.ToEnumTypeName());

        public static string ToRepr(this IntPtr self) => string.Format("[0x{0:x16}]", self.ToInt64());

        public static uint ToUIntMilliseconds(this TimeSpan timeSpan) => (uint)unchecked(timeSpan.TotalMilliseconds.ToLong());

        public static WM ToWM(this UserMessage userMessage) => (WM)((uint)WM.USER + (uint)userMessage);

        public static Rectangle GetRectangleFromRect(this Rect rect)
        {
            var width = rect.Right - rect.Left;
            var height = rect.Bottom - rect.Top;
            var left = rect.Left;
            var top = rect.Top;
            return new Rectangle(width, height, left, top);
        }

        public static int ToInt32(this IntPtr intPtr) => unchecked((int)intPtr.ToInt64());

        public static string ToRepr(this bool self) => self ? "true" : "false";
    }
}