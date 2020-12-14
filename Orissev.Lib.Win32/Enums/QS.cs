using System;

namespace Orissev.Win32.Enums
{
    [Flags]
    public enum QS : uint
    {
        KEY = 0x1,
        MOUSEMOVE = 0x2,
        MOUSEBUTTON = 0x4,
        MOUSE = MOUSEMOVE | MOUSEBUTTON,
        INPUT = MOUSE | KEY,
        POSTMESSAGE = 0x8,
        TIMER = 0x10,
        PAINT = 0x20,
        SENDMESSAGE = 0x40,
        HOTKEY = 0x80,
        REFRESH = HOTKEY | KEY | MOUSEBUTTON | PAINT,
        ALLEVENTS = INPUT | POSTMESSAGE | TIMER | PAINT | HOTKEY,
        ALLINPUT = SENDMESSAGE | PAINT | TIMER | POSTMESSAGE | MOUSEBUTTON | MOUSEMOVE | HOTKEY | KEY,
        ALLPOSTMESSAGE = 0x100,
        RAWINPUT = 0x400
    }
}