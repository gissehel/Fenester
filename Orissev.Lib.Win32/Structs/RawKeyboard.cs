using Orissev.Win32.Enums;
using System.Runtime.InteropServices;

namespace Orissev.Win32.Structs
{
    /// <summary>
    /// Value type for raw input from a keyboard.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RawKeyboard
    {
        /// <summary>Scan code for key depression.</summary>
        public short MakeCode;

        /// <summary>Scan code information.</summary>
        public RawKeyboardFlags Flags;

        /// <summary>Reserved.</summary>
        public ushort Reserved;

        /// <summary>Virtual key code.</summary>
        public VirtualKeys VirtualKey;

        /// <summary>Corresponding window message.</summary>
        public WM Message;

        /// <summary>Extra information.</summary>
        public ulong ExtraInformation;
    }
}