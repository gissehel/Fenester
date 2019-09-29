using Fenester.Lib.Win.Domain.Os;
using Fenester.Lib.Win.Service.Helpers;
using System;
using System.Collections.Generic;

namespace Fenester.Lib.Win.Service
{
    public class Win32Monitor
    {
        public static IEnumerable<Screen> GetMonitors()
        {
            List<Screen> result = new List<Screen>();
            int id = 0;
            Win32.EnumDisplayMonitors
                (
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData) =>
                    {
                        id++;
                        var screen = new Screen
                        {
                            Index = id,
                            Id = string.Format("Screen_{0}", id),
                            Name = string.Format("Screen {0}", id),
                            Rectangle = lprcMonitor.GetRectangleFromRect()
                        };
                        result.Add(screen);
                        return true;
                    },

                    IntPtr.Zero
                );

            return result;
        }
    }
}