using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service.Helpers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}