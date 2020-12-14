using System;

namespace Orissev.Win32.Enums
{
    [Flags]
    public enum MWMO : uint
    {
        NONE = 0,
        WAITALL = 1,
        ALERTABLE = 2,
        INPUTAVAILABLE = 4,
    }
}