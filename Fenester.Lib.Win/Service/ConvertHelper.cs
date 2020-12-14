using Fenester.Lib.Core.Enums;
using Fenester.Lib.Graphical.Domain.Graphical;
using Fenester.Lib.Win.Enums;
using Orissev.Win32.Enums;
using Orissev.Win32.Structs;

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

        public static WM ToWM(this UserMessage userMessage) => (WM)((uint)WM.USER + (uint)userMessage);

        public static Rectangle GetRectangleFromRect(this Rect rect)
        {
            var width = rect.Right - rect.Left;
            var height = rect.Bottom - rect.Top;
            var left = rect.Left;
            var top = rect.Top;
            return new Rectangle(width, height, left, top);
        }
    }
}