using Orissev.Win32.Enums;
using System.Runtime.InteropServices;

namespace Orissev.Win32.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Input
    {
        public InputType type;
        public InputUnion U;
        public static int Size
        {
            get { return Marshal.SizeOf(typeof(Input)); }
        }
    }
}
