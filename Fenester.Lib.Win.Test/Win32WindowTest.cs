using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Test.Tools.Win;
using Fenester.Lib.Win.Domain.Os;
using Fenester.Lib.Win.Service.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orissev.Win32;
using Orissev.Win32.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fenester.Lib.Win.Test
{
    [TestClass]
    public class Win32WindowTest : DebuggableTest
    {
        protected override void InitTracesPost()
        {
            Win32Window.Tracable = this;
        }

        protected override void UninitTracesPre()
        {
            Win32Window.Tracable = null;
        }

        [TestMethod]
        public void CompareGetHandles()
        {
            TraceFile.SetName("CompareGetHandles");
            var list1 = new List<IntPtr>();
            var list2 = new List<IntPtr>();

            var intPtrComparer = new IntPtrComparer();

            list1.AddRange(Win32Window.GetHandles());
            list2.AddRange(Win32Window.GetHandles2());

            list1.Sort(intPtrComparer);
            list2.Sort(intPtrComparer);

            TraceFile
                .Get("CompareGetHandles-List1")
                .Each(list1, (trace, handle) => trace.OutLine(handle.ToRepr()))
                .Close()
            ;
            TraceFile
                .Get("CompareGetHandles-List2")
                .Each(list2, (trace, handle) => trace.OutLine(handle.ToRepr()))
                .Close()
            ;

            CollectionAssert.AreEqual(list1, list2);
        }

        [TestMethod]
        public void GetOpenWindowsTest()
        {
            TraceFile.SetName("GetOpenWindowsTest");
            var windowsByHandle = Win32Window.GetOpenWindows();

            var windows = windowsByHandle
                .Values.OrderBy(window => (window.Id as WindowId).Handle.ToInt64())
                .Select(window => window as IWindow);

            foreach (var window in windows)
            {
                this.LogLine
                (
                    "{0}:[{1}] ({2}) [{3}]",
                    window.Canonical,
                    window.Title,
                    window.Class,
                    window.OsVisibility
                );
            }
        }

        private void ChangeWindow(Action<IntPtr> change, Action<IntPtr, IWindow> afterChange)
        {
            var windowsByHandle = Win32Window.GetOpenWindows();

            var windows = windowsByHandle
                .Values.OrderBy(window => (window.Id as WindowId).Handle.ToInt64())
                .Select(window => window as IWindow)
                .Where(window => window.Class == "TTOTAL_CMD")
            ;

            foreach (var window in windows)
            {
                var handle = (window.Id as WindowId).Handle;
                {
                    if (Win32Window.GetWindowStyles(handle, out WS styles, out WS_EX extStyles))
                    {
                        this.LogLine
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
                        this.LogLine
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
        }

        [TestMethod]
        public void ChangeWindow()
        {
            TraceFile.SetName("ChangeWindow");
            ChangeWindow
            (
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
            TraceFile.SetName("ChangeWindowBackTest");
            ChangeWindow
            (
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
                string.Join("|", FlagsExtension.FlagAnalyserWS.GetNames(style)),
                string.Join("|", FlagsExtension.FlagAnalyserWSEX.GetNames(exStyle))
            );
    }
}