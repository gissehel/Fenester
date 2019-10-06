using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service.Helpers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int x;
        public int y;
    }
}