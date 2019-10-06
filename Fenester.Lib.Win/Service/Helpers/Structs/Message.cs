using System;
using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service.Helpers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Message
    {
        public IntPtr handle;
        public WM message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public Point pt;
    }
}