using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Win.Domain.Os;
using Fenester.Lib.Win.Service;
using Fenester.Lib.Win.Service.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fenester.Lib.Win.Test
{
    [TestClass]
    public class Win32WindowTest
    {
        private class IntPtrComparer : IComparer<IntPtr>
        {
            public int Compare(IntPtr x, IntPtr y) => x.ToInt64().CompareTo(y.ToInt64());
        }

        [TestMethod]
        public void CompareGetHandles()
        {
            var list1 = new List<IntPtr>();
            var list2 = new List<IntPtr>();

            var intPtrComparer = new IntPtrComparer();

            list1.AddRange(Win32Window.GetHandles());
            list2.AddRange(Win32Window.GetHandles2());

            list1.Sort(intPtrComparer);
            list2.Sort(intPtrComparer);

            TraceFile
                .Get("List1")
                .Each(list1, (trace, handle) => trace.Out("{0:x16}", handle))
                .Close()
            ;
            TraceFile
                .Get("List2")
                .Each(list2, (trace, handle) => trace.Out("{0:x16}", handle))
                .Close()
            ;

            CollectionAssert.AreEqual(list1, list2);
        }

        [TestMethod]
        public void GetOpenWindowsTest()
        {
            var windowsByHandle = Win32Window.GetOpenWindows();

            var windows = windowsByHandle
                .Values.OrderBy(window => (window.Id as WindowId).Handle.ToInt64())
                .Select(window => window as IWindow);

            TraceFile
                .Get("Windows")
                .Each(
                    windows,
                    (trace, window) => trace.Out
                        (
                            "{0}:[{1}] ({2}) [{3}]",
                            window.Canonical,
                            window.Title,
                            window.Class,
                            window.OsVisibility
                        )
                )
                .Close()
            ;
        }

        [TestMethod]
        public void ChangeWindowTest()
        {
            var windowsByHandle = Win32Window.GetOpenWindows();

            var windows = windowsByHandle
                .Values.OrderBy(window => (window.Id as WindowId).Handle.ToInt64())
                .Select(window => window as IWindow)
                .Where(window => window.Class == "TTOTAL_CMD")
            ;

            TraceFile
                .Get("Windows_to_change")
                .Each(
                    windows,
                    (trace, window) =>
                    {
                        var handle = (window.Id as WindowId).Handle;
                        {
                            if (Win32Window.GetWindowStyles(handle, out uint styles, out uint extStyles))
                            {
                                trace.Out
                                    (
                                        "{0}:[{1}] ({2}) [{3}] : {4} - {5}",
                                        window.Canonical,
                                        window.Title,
                                        window.Class,
                                        window.OsVisibility,
                                        styles,
                                        extStyles
                                   );
                            }
                        }
                        Win32Window.SetWindowStyles(handle, 0, Win32.WS_THICKFRAME | Win32.WS_BORDER, 0, 0);
                        {
                            if (Win32Window.GetWindowStyles(handle, out uint styles, out uint extStyles))
                            {
                                trace.Out
                                    (
                                        "{0}:[{1}] ({2}) [{3}] : {4} - {5}",
                                        window.Canonical,
                                        window.Title,
                                        window.Class,
                                        window.OsVisibility,
                                        styles,
                                        extStyles
                                   );
                            }
                        }

                        Win32.MoveWindow(handle, (1920 * 4) / 3, 0, 1920 / 3, 1080, true);
                    }
                )
                .Close()
            ;
        }

        [TestMethod]
        public void ChangeWindowBackTest()
        {
            var windowsByHandle = Win32Window.GetOpenWindows();

            var windows = windowsByHandle
                .Values.OrderBy(window => (window.Id as WindowId).Handle.ToInt64())
                .Select(window => window as IWindow)
                .Where(window => window.Class == "TTOTAL_CMD")
            ;

            TraceFile
                .Get("Windows_to_change_back")
                .Each(
                    windows,
                    (trace, window) =>
                    {
                        var handle = (window.Id as WindowId).Handle;
                        {
                            if (Win32Window.GetWindowStyles(handle, out uint styles, out uint extStyles))
                            {
                                trace.Out
                                    (
                                        "{0}:[{1}] ({2}) [{3}] : {4} - {5}",
                                        window.Canonical,
                                        window.Title,
                                        window.Class,
                                        window.OsVisibility,
                                        styles,
                                        extStyles
                                   );
                            }
                        }
                        Win32Window.SetWindowStyles(handle, Win32.WS_THICKFRAME | Win32.WS_BORDER, 0, 0, 0);
                        {
                            if (Win32Window.GetWindowStyles(handle, out uint styles, out uint extStyles))
                            {
                                trace.Out
                                    (
                                        "{0}:[{1}] ({2}) [{3}] : {4} - {5}",
                                        window.Canonical,
                                        window.Title,
                                        window.Class,
                                        window.OsVisibility,
                                        styles,
                                        extStyles
                                   );
                            }
                        }

                        Win32.MoveWindow(handle, 1920 + 300, 200, 1100, 800, true);
                    }
                )
                .Close()
            ;
        }
    }
}