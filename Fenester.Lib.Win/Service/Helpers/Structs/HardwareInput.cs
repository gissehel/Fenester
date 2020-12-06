using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service.Helpers.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HardwareInput
    {
        public int uMsg;
        public short wParamL;
        public short wParamH;
    }
}
