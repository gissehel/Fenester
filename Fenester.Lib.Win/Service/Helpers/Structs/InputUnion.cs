﻿using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service.Helpers.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct InputUnion
    {
        [FieldOffset(0)]
        public MOUSEINPUT mi;
        [FieldOffset(0)]
        public KEYBDINPUT ki;
        [FieldOffset(0)]
        public HARDWAREINPUT hi;
    }
}
