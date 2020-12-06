using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fenester.Lib.Win.Service.Helpers.Enums
{
    [Flags]
    public enum KEYEVENTF : uint
    {
        NONE = 0x0000,
        EXTENDEDKEY = 0x0001,
        KEYUP = 0x0002,
        SCANCODE = 0x0008,
        UNICODE = 0x0004
    }
}
