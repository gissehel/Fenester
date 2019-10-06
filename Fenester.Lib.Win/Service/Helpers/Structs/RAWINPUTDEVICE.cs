using Fenester.Lib.Win.Service.Helpers.Enums;
using System;
using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service.Helpers.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputDevice
    {
        /// <summary>Top level collection Usage page for the raw input device.</summary>
        public HIDUsagePage UsagePage;

        /// <summary>Top level collection Usage for the raw input device. </summary>
        public HIDUsage Usage;

        /// <summary>Mode flag that specifies how to interpret the information provided by UsagePage and Usage.</summary>
        public RIDEV Flags;

        /// <summary>Handle to the target device. If NULL, it follows the keyboard focus.</summary>
        public IntPtr WindowHandle;
    }
}