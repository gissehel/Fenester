using System;
using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service.Helpers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowPlacement
    {
        [Flags]
        public enum Flags : uint
        {
            WPF_ASYNCWINDOWPLACEMENT = 0x0004,
            WPF_RESTORETOMAXIMIZED = 0x0002,
            WPF_SETMINPOSITION = 0x0001
        }

        public uint length;

        public Flags flags;//uint flags;

        public uint showCmd;

        public Point ptMinPosition;

        public Point ptMaxPosition;

        public Rect rcNormalPosition;
    }
}