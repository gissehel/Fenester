using System;

namespace Fenester.Lib.Win.Service.Helpers
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