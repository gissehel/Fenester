using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Enums;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Win.Domain.Os;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Fenester.Lib.Win.Service.Helpers
{
    public static class Win32Window
    {
        public static ITracable Tracable { get; set; }

        public static bool GetWindowProps(Window window)
        {
            try
            {
                var handle = window.Handle;

                window.Title = GetWindowTitle(handle);

                window.Class = GetClassName(handle);

                Rect? windowRect = null;

                if (Win32.GetWindowRect(handle, out Rect rect))
                {
                    windowRect = rect;
                }

                if (Win32.GetWindowPlacement(handle, out WindowPlacement windowPlacement))
                {
                    switch (windowPlacement.showCmd)
                    {
                        case SW.SHOWMINIMIZED:
                            window.OsVisibility = Visibility.Minimized;
                            window.RectangleCurrent = null;
                            window.Rectangle = windowPlacement.normalPosition.GetRectangleFromRect();
                            break;

                        case SW.SHOWMAXIMIZED:
                            window.OsVisibility = Visibility.Maximised;
                            if (windowRect.HasValue)
                            {
                                window.RectangleCurrent = windowRect.Value.GetRectangleFromRect();
                            }
                            window.Rectangle = windowPlacement.normalPosition.GetRectangleFromRect();
                            break;

                        default:
                            var isVisible = Win32.IsWindowVisible(handle);
                            if (isVisible)
                            {
                                window.OsVisibility = Visibility.Normal;
                                if (windowRect.HasValue)
                                {
                                    window.RectangleCurrent = windowRect.Value.GetRectangleFromRect();
                                }
                                window.Rectangle = windowPlacement.normalPosition.GetRectangleFromRect();
                            }
                            else
                            {
                                window.OsVisibility = Visibility.None;
                                window.Rectangle = null;
                            }
                            break;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void AddWindow(Dictionary<IntPtr, Window> windows, IntPtr handle, IntPtr shellWindow)
        {
            var window = new Window(handle);

            if (handle == shellWindow)
            {
                window.Category = WindowCategory.Shell;
            }
            else
            {
                window.Category = WindowCategory.Normal;
            }

            GetWindowProps(window);

            //if (window.OsVisibility == Visibility.None)
            //{
            //    return;
            //}

            //if
            //    (
            //        (window.Rectangle == null)
            //        ||
            //        (
            //            (window.Rectangle != null)
            //            &&
            //            (
            //                (window.Rectangle.Size.Width == 0)
            //                ||
            //                (window.Rectangle.Size.Height == 0)
            //            )
            //        )
            //    )
            //{
            //    return;
            //}

            windows[handle] = window;
        }

        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        public static IDictionary<IntPtr, Window> GetOpenWindows()
        {
            IntPtr shellWindow = Win32.GetShellWindow();
            IntPtr desktopWindow = Win32.GetDesktopWindow();
            Dictionary<IntPtr, Window> windows = new Dictionary<IntPtr, Window>();

            AddWindow(windows, desktopWindow, shellWindow);
            AddWindow(windows, shellWindow, IntPtr.Zero);

            Win32.EnumWindows((IntPtr handle, int lParam) =>
            {
                AddWindow(windows, handle, IntPtr.Zero);
                return true;
            }, 0);

            return windows;
        }

        public static IEnumerable<IntPtr> GetHandles2()
        {
            List<IntPtr> windows = new List<IntPtr>();

            Win32.EnumWindows((IntPtr handle, int lParam) => { windows.Add(handle); return true; }, 0);

            return windows;
        }

        private static string GetClassName(IntPtr handle)
        {
            // Pre-allocate 256 characters, since this is the maximum class name length.
            StringBuilder className = new StringBuilder(256);
            int nRet = Win32.GetClassName(handle, className, className.Capacity);
            if (nRet != 0)
            {
                return className.ToString();
            }
            else
            {
                return null;
            }
        }

        private static string GetWindowTitle(IntPtr handle)
        {
            int length = Win32.GetWindowTextLength(handle);
            if (length != 0)
            {
                StringBuilder builder = new StringBuilder(length);
                Win32.GetWindowText(handle, builder, length + 1);
                return builder.ToString();
            }
            return null;
        }

        public static IEnumerable<IntPtr> GetHandles()
        {
            Process[] processes = Process.GetProcesses();

            var handleList = new List<IntPtr>();

            foreach (Process p in processes)
            {
                IEnumerable<IntPtr> w = GetRootWindowsOfProcess(p.Id);
                handleList.AddRange(w);
            }

            return handleList;
        }

        private static IEnumerable<IntPtr> GetRootWindowsOfProcess(int pid)
        {
            IEnumerable<IntPtr> rootWindows = GetChildWindows(IntPtr.Zero);
            var dsProcRootWindows = new List<IntPtr>();
            foreach (IntPtr hWnd in rootWindows)
            {
                Win32.GetWindowThreadProcessId(hWnd, out uint lpdwProcessId);
                if (lpdwProcessId == pid)
                {
                    dsProcRootWindows.Add(hWnd);
                }
            }
            return dsProcRootWindows;
        }

        private static IEnumerable<IntPtr> GetChildWindows(IntPtr parent)
        {
            var result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                var childProc = new Win32Callback(EnumWindow);
                Win32.EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                {
                    listHandle.Free();
                }
            }
            return result;
        }

        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            if (gch.Target is List<IntPtr> list)
            {
                list.Add(handle);
                return true;
            }
            throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
        }

        public static bool GetWindowStyles(IntPtr handle, out WS styles, out WS_EX extStyles)
        {
            try
            {
                styles = Win32.GetWindowLong(handle, GWL.STYLE).ToWS();
                extStyles = Win32.GetWindowLong(handle, GWL.EXSTYLE).ToWS_EX();
                return true;
            }
            catch
            {
                styles = WS.NONE;
                extStyles = WS_EX.NONE;
                return false;
            }
        }

        public static bool SetWindowStyles(IntPtr handle, WS styles, WS_EX extStyles)
        {
            try
            {
                Win32.SetWindowLong(handle, GWL.STYLE, new IntPtr((uint)styles));
                Win32.SetWindowLong(handle, GWL.EXSTYLE, new IntPtr((uint)extStyles));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ChangeWindowStyles(IntPtr handle, WS stylesToAdd, WS stylesToRemove, WS_EX extStylesToAdd, WS_EX extStyleToRemove)
        {
            try
            {
                var styles = Win32.GetWindowLong(handle, GWL.STYLE).ToWS();
                var extStyles = Win32.GetWindowLong(handle, GWL.EXSTYLE).ToWS_EX();

                styles |= stylesToAdd;
                styles &= ~stylesToRemove;
                extStyles |= extStylesToAdd;
                extStyles &= ~extStyleToRemove;

                Win32.SetWindowLong(handle, GWL.STYLE, new IntPtr((uint)styles));
                Win32.SetWindowLong(handle, GWL.EXSTYLE, new IntPtr((uint)extStyles));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool FocusWindow(IntPtr handle)
        {
            var style = Win32.GetWindowLong(handle, GWL.STYLE).ToWS();

            // Minimize and restore to be able to make it active.
            if ((style & WS.MINIMIZE) == WS.MINIMIZE)
            {
                Win32.ShowWindow(handle, SW.RESTORE);
            }

            uint currentlyFocusedWindowProcessId = Win32.GetWindowThreadProcessId(Win32.GetForegroundWindow(), IntPtr.Zero);
            uint appThread = Win32.GetCurrentThreadId();

            if (currentlyFocusedWindowProcessId != appThread)
            {
                Win32.AttachThreadInput(currentlyFocusedWindowProcessId, appThread, true);
                Win32.BringWindowToTop(handle);
                Win32.ShowWindow(handle, SW.SHOW);
                Win32.AttachThreadInput(currentlyFocusedWindowProcessId, appThread, false);
            }
            else
            {
                Win32.BringWindowToTop(handle);
                Win32.ShowWindow(handle, SW.SHOW);
            }

            return true;
        }

        public static void MoveWindowAndRedraw(IntPtr handle, IRectangle rectangle)
        {
            Win32.MoveWindow(handle, rectangle.Position.Left, rectangle.Position.Top, rectangle.Size.Width, rectangle.Size.Height, true);
            Win32.DrawMenuBar(handle);
        }

        public static IntPtr CreateWindow(Func<Message, IntPtr> processMessage, string className, string title = null)
        {
            var windowProc = new WindowProc((IntPtr handleWindow, WM wm, IntPtr wParam, IntPtr lParam) =>
            {
                IntPtr result = IntPtr.Zero;
                if (processMessage != null)
                {
                    var message = new Message
                    {
                        handle = handleWindow,
                        message = wm,
                        wParam = wParam,
                        lParam = lParam,
                    }; ;
                    result = processMessage(message);
                }
                if (result == IntPtr.Zero)
                {
                    return Win32.DefWindowProc(handleWindow, wm, wParam, lParam);
                }
                else
                {
                    return result;
                }
            });

            var windowClassEx = new WindowClassEx()
            {
                size = Marshal.SizeOf(typeof(WindowClassEx)),
                style = CS.HREDRAW | CS.VREDRAW,
                windowProc = Marshal.GetFunctionPointerForDelegate(windowProc),
                classExtra = 0,
                windowExtra = 0,
                handleInstance = IntPtr.Zero,
                handleIcon = IntPtr.Zero,
                handleCursor = IntPtr.Zero,
                handlebrBackground = IntPtr.Zero,
                menuName = null,
                className = className,
                handleIconSm = IntPtr.Zero,
            };

            Tracable.LogLine("windowClassEx {0}", windowClassEx.ToString());

            var registeredClassEx = Win32.RegisterClassEx(ref windowClassEx);
            Tracable.LogLine("registeredClassEx {0}", registeredClassEx.ToString());
            if (registeredClassEx == 0)
            {
                var error = Win32.GetLastError();
                Tracable.LogLine("error {0}", error.ToRepr());
                Win32.UnregisterClass(className, IntPtr.Zero);
                return IntPtr.Zero;
            }

            var handle = Win32.CreateWindowEx
                (
                    WS_EX.NONE,
                    registeredClassEx,
                    title ?? windowClassEx.className,
                    WS.NONE,
                    0,
                    0,
                    0,
                    0,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    windowClassEx.handleInstance,
                    IntPtr.Zero
                );
            Tracable.LogLine("handle {0}", handle.ToRepr());

            if (handle == null)
            {
                var error = Win32.GetLastError();
                Tracable.LogLine("error {0}", error.ToRepr());
            }
            return handle;
        }

        public static void ListenMessages(IntPtr handle, TimeSpan timeout, Func<Message, IntPtr> onMessage, Func<bool> shouldContinue)
        {
            DateTime start = DateTime.Now;

            var remaining = timeout - (DateTime.Now - start);
            while ((timeout == TimeSpan.Zero || remaining > TimeSpan.Zero) && (shouldContinue == null || shouldContinue()))
            {
                Tracable.LogLine("while : remaining = [{0}]", remaining.TotalMilliseconds);
                if (Win32.MsgWaitForMultipleObjectsEx(0, new IntPtr[] { }, remaining.ToUIntMilliseconds(), QS.ALLEVENTS, MWMO.WAITALL) == WAIT.OBJECT_0)
                {
                    Tracable.LogLine("MsgWaitForMultipleObjectsEx");
                    while (Win32.PeekMessage(out Message message, handle, 0, 0, PM.REMOVE))
                    {
                        Tracable.LogLine("PeekMessage => {0}", message.message.ToRepr());
                        var result = onMessage(message);
                        Win32.TranslateMessage(ref message);
                        Win32.DispatchMessage(ref message);
                    }
                }
                remaining = timeout - (DateTime.Now - start);
                Tracable.LogLine("end while : remaining = [{0}]", remaining.TotalMilliseconds);
            }
        }
    }
}