using System;

namespace Fenester.Lib.Core.Enums
{
    [Flags]
    public enum KeyModifier
    {
        None = 0,
        Ctrl = 1,
        Shift = 2,
        Alt = 4,
        Win = 8,
    }
}