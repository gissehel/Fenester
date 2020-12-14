using Orissev.Win32.Enums;
using System.Runtime.InteropServices;

namespace Orissev.Win32.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowPlacement
    {
        public uint length;

        public WPF flags;

        public SW showCmd;

        public Point minPosition;

        public Point maxPosition;

        public Rect normalPosition;
    }
}