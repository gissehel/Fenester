using Orissev.Win32.Structs;
using System.Runtime.InteropServices;

namespace Orissev.Win32.Structs
{
    /// <summary>
    /// Value type for raw input.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct RawInput
    {
        /// <summary>Header for the data.</summary>
        [FieldOffset(0)]
        public RawInputHeader Header;

        ///// <summary>Mouse raw input data.</summary>
        //[FieldOffset(16)]
        //public RawInputMouse Mouse;

        /// <summary>Keyboard raw input data.</summary>
        [FieldOffset(16)]
        public RawKeyboard Keyboard;

        ///// <summary>HID raw input data.</summary>
        //[FieldOffset(16)]
        //public RawInputHid Hid;
    }
}