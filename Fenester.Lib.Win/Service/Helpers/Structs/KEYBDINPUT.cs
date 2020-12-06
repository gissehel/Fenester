using Fenester.Lib.Win.Service.Helpers.Enums;
using System;
using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service.Helpers.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        public VirtualKeys wVk;
        public ScanCode wScan;
        public KEYEVENTF dwFlags;
        public int time;
        public UIntPtr dwExtraInfo;
    }
}
