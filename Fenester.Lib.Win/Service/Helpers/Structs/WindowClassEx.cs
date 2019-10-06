using System;
using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service.Helpers
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct WindowClassEx
    {
        // [MarshalAs(UnmanagedType.U4)]
        public int size;

        // [MarshalAs(UnmanagedType.U4)]
        public CS style;

        public IntPtr windowProc;
        public int classExtra;
        public int windowExtra;
        public IntPtr handleInstance;
        public IntPtr handleIcon;
        public IntPtr handleCursor;
        public IntPtr handlebrBackground;
        public string menuName;
        public string className;
        public IntPtr handleIconSm;
    }
}