using Fenester.Lib.Core.Service;
using Fenester.Lib.Win.Domain.Os;
using System;
using System.Collections.Generic;

namespace Fenester.Lib.Win.Service.Helpers
{
    public class Win32Monitor
    {
        public static ITracable Tracable { get; set; }

        public static IEnumerable<Screen> GetMonitors()
        {
            List<Screen> result = new List<Screen>();
            int id = 0;
            Win32.EnumDisplayMonitors
                (
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (IntPtr handleMonitor, IntPtr hdcMonitor, ref Rect rectangleMonitor, IntPtr dwData) =>
                    {
                        id++;
                        var screen = new Screen
                        {
                            Index = id,
                            Id = string.Format("Screen_{0}", id),
                            Name = string.Format("Screen {0}", id),
                            Rectangle = rectangleMonitor.GetRectangleFromRect()
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