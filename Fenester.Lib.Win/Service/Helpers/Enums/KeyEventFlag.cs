using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fenester.Lib.Win.Service.Helpers.Enums
{
    [Flags]
    public enum KeyEventFlag : uint
    {
        None = 0x0000,
        ExtendedKey = 0x0001,
        KeyUp = 0x0002,
        ScanCode = 0x0008,
        Unicode = 0x0004
    }
}
