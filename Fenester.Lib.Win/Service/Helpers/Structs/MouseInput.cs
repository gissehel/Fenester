using Fenester.Lib.Win.Service.Helpers.Enums;
using System;
using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service.Helpers.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseInput
    {
        public int dx;
        public int dy;
        public int mouseData;
        public MOUSEEVENTF dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }
}
