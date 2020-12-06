using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service.Helpers.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct InputUnion
    {
        [FieldOffset(0)]
        public MouseInput mi;
        [FieldOffset(0)]
        public KeyboardInput ki;
        [FieldOffset(0)]
        public HardwareInput hi;
    }
}
