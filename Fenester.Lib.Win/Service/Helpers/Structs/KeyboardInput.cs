using Fenester.Lib.Win.Service.Helpers.Enums;
using System;
using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service.Helpers.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardInput
    {
        public VirtualKeys wVk;
        public ScanCode wScan;
        public KeyEventFlag dwFlags;
        public int time;
        public UIntPtr dwExtraInfo;
    }
}
