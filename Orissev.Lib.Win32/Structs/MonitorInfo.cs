using System.Runtime.InteropServices;

namespace Orissev.Win32.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public class MonitorInfo
    {
        public uint Size;
        public Rect Monitor;
        public Rect Work;
        public uint Flags;

        public MonitorInfo()
        {
            Monitor = new Rect();
            Work = new Rect();

            Size = (uint)Marshal.SizeOf(typeof(MonitorInfo));
            Flags = 0;
        }
    }
}