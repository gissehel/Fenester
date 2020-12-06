using Fenester.Lib.Win.Service.Helpers.Enums;
using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service.Helpers.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Input
    {
        internal InputType type;
        internal InputUnion U;
        internal static int Size
        {
            get { return Marshal.SizeOf(typeof(Input)); }
        }
    }
}
