using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service.Helpers
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