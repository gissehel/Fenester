﻿using System;

namespace Orissev.Win32.Enums
{
    [Flags]
    public enum WPF : uint
    {
        ASYNCWINDOWPLACEMENT = 0x0004,
        RESTORETOMAXIMIZED = 0x0002,
        SETMINPOSITION = 0x0001
    }
}