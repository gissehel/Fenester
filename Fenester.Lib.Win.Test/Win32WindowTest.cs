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

        private void ChangeWindow(string traceName, Action<IntPtr> change, Action<IntPtr, IWindow> afterChange)
        {
            var windowsByHandle = Win32Window.GetOpenWindows();

            var windows = windowsByHandle
                .Values.OrderBy(window => (window.Id as WindowId).Handle.ToInt64())
                .Select(window => window as IWindow)
                .Where(window => window.Class == "TTOTAL_CMD")
            ;

            TraceFile
                .Get(traceName)
                .Each(
                    windows,
                    (trace, window) =>
                    {
                        var handle = (window.Id as WindowId).Handle;
                        {
                            if (Win32Window.GetWindowStyles(handle, out WS styles, out WS_EX extStyles))
                            {
                                trace.Out
                                    (
                                        "{0}:[{1}] ({2}) [{3}] : {4} - {5} - {6}",
                                        window.Canonical,
                                        window.Title,
                                        window.Class,
                                        window.OsVisibility,
                                        styles,
                                        extStyles,
                                        GetStyles(styles, extStyles)
                                   );
                            }
                        }
                        change(handle);
                        {
                            if (Win32Window.GetWindowStyles(handle, out WS styles, out WS_EX extStyles))
                            {
                                trace.Out
                                    (
                                        "{0}:[{1}] ({2}) [{3}] : {4} - {5} - {6}",
                                        window.Canonical,
                                        window.Title,
                                        window.Class,
                                        window.OsVisibility,
                                        styles,
                                        extStyles,
                                        GetStyles(styles, extStyles)
                                   );
                            }
                        }
                        afterChange(handle, window);
                    }
                )
                .Close()
            ;
        }

        [TestMethod]
        public void ChangeWindowTest()
        {
            ChangeWindow
            (
                "Windows_to_change",
                (handle) =>
                {
                    Win32Window.ChangeWindowStyles(handle, 0, WS.MINIMIZEBOX | WS.MAXIMIZEBOX | WS.THICKFRAME, 0, 0);
                },
                (handle, window) =>
                {
                    Win32Window.MoveWindowAndRedraw(handle, window.RectangleCurrent);
                    Win32Window.FocusWindow(handle);
                }
            );
        }

        [TestMethod]
        public void ChangeWindowBackTest()
        {
            ChangeWindow
            (
                "Windows_to_change_back",
                (handle) =>
                {
                    // Win32Window.SetWindowStyles(handle, WS.BORDER, WS.POPUPWINDOW, 0, 0);
                    // Win32Window.SetWindowStyles(handle, 0, 0, 0, WS_EX.TOOLWINDOW);
                    Win32.SetWindowLong(handle, GWL.STYLE, new IntPtr(382664704));
                    Win32.SetWindowLong(handle, GWL.EXSTYLE, new IntPtr(327936));
                },
                (handle, window) =>
                {
                    Win32Window.MoveWindowAndRedraw(handle, window.RectangleCurrent);
                    Win32Window.FocusWindow(handle);
                }
            );
        }

        private string GetStyles(WS style, WS_EX exStyle)
            => string.Format
            (
                "[{0}] [{1}]",
                string.Join("|", Flags.FlagAnalyserWS.GetNames(style)),
                string.Join("|", Flags.FlagAnalyserWSEX.GetNames(exStyle))
            );
    }
}