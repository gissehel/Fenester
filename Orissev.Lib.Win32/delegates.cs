﻿using Orissev.Win32.Enums;
using Orissev.Win32.Structs;
using System;

namespace Orissev.Win32
{
    public delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

    public delegate bool Win32Callback(IntPtr handle, IntPtr lParam);

    public delegate bool EnumWindowsProc(IntPtr handleWindow, int lParam);

    public delegate IntPtr WindowProc(IntPtr handleWindow, WM wm, IntPtr wParam, IntPtr lParam);

    public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);
}