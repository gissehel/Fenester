using System;

namespace Fenester.Lib.Win.Service.Helpers
{
    public delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

    public delegate bool Win32Callback(IntPtr handle, IntPtr lParam);

    public delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);
}