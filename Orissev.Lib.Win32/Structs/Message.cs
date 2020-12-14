using Orissev.Win32.Enums;
using System;
using System.Runtime.InteropServices;

namespace Orissev.Win32.Structs
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