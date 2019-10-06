using System;

namespace Fenester.Lib.Win.Service.Helpers
{
    [Flags]
    public enum WAIT : uint
    {
        OBJECT_0 = 0,
        OBJECT_1 = OBJECT_0 + 1,
        OBJECT_2 = OBJECT_0 + 2,
        OBJECT_3 = OBJECT_0 + 3,
        OBJECT_4 = OBJECT_0 + 4,
        OBJECT_5 = OBJECT_0 + 5,
        OBJECT_6 = OBJECT_0 + 6,
        OBJECT_7 = OBJECT_0 + 7,
        OBJECT_8 = OBJECT_0 + 8,
        OBJECT_9 = OBJECT_0 + 9,
        OBJECT_10 = OBJECT_0 + 10,

        ABANDONED_0 = 0x80,
        ABANDONED_1 = ABANDONED_0 + 1,
        ABANDONED_2 = ABANDONED_0 + 2,
        ABANDONED_3 = ABANDONED_0 + 3,
        ABANDONED_4 = ABANDONED_0 + 4,
        ABANDONED_5 = ABANDONED_0 + 5,
        ABANDONED_6 = ABANDONED_0 + 6,
        ABANDONED_7 = ABANDONED_0 + 7,
        ABANDONED_8 = ABANDONED_0 + 8,
        ABANDONED_9 = ABANDONED_0 + 9,
        ABANDONED_10 = ABANDONED_0 + 10,

        IO_COMPLETION = 0xC0,
        TIMEOUT = 258,
        FAILED = 0xFFFFFFFF,
    }
}