using System.Runtime.InteropServices;

namespace Orissev.Win32.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HardwareInput
    {
        public int uMsg;
        public short wParamL;
        public short wParamH;
    }
}
