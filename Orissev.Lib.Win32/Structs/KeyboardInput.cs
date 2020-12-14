using Orissev.Win32.Enums;
using System;
using System.Runtime.InteropServices;

namespace Orissev.Win32.Structs
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
