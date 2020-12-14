﻿using System;

namespace Orissev.Win32.Enums
{
    [Flags]
    public enum MOUSEEVENTF : uint
    {
        ABSOLUTE = 0x8000,
        LEFTDOWN = 0x0002,
        LEFTUP = 0x0004,
        MIDDLEDOWN = 0x0020,
        MIDDLEUP = 0x0040,
        MOVE = 0x0001,
        RIGHTDOWN = 0x0008,
        RIGHTUP = 0x0010,
        XDOWN = 0x0080,
        XUP = 0x0100,
        WHEEL = 0x0800,
        HWHEEL = 0x01000,
    }
}
