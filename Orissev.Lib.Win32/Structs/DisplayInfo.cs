using System.Runtime.InteropServices;

namespace Orissev.Win32.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public class DisplayInfo
    {
        public string Availability { get; set; }
        public string ScreenHeight { get; set; }
        public string ScreenWidth { get; set; }
        public Rect MonitorArea { get; set; }
        public Rect WorkArea { get; set; }
    }
}