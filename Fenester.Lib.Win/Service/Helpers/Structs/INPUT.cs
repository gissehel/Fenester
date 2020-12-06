using Fenester.Lib.Win.Service.Helpers.Enums;
using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service.Helpers.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        internal INPUT_TYPE type;
        internal InputUnion U;
        internal static int Size
        {
            get { return Marshal.SizeOf(typeof(INPUT)); }
        }
    }
}
