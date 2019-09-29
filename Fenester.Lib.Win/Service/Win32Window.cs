using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Enums;
using Fenester.Lib.Win.Domain.Os;
using Fenester.Lib.Win.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Fenester.Lib.Win.Service
{
    public class Win32Window
    {
        private static void AddWindow(Dictionary<IntPtr, IInternalWindow> windows, IntPtr handle, IntPtr shellWindow)
        {
            var isVisible = Win32.IsWindowVisible(handle);
            if (!isVisible)
            {
                return;
            }
            var window = new Window(handle);

            if (handle == shellWindow)
            {
                window.Category = WindowCategory.Shell;
            }
            else
            {
                window.Category = WindowCategory.Normal;
            }

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
                    case Win32.SW_SHOWMINIMIZED:
                        window.OsVisibility = Visibility.Minimized;
                        window.RectangleCurrent = null;
                        window.Rectangle = Win32Helper.GetRectangleFromRect(windowPlacement.rcNormalPosition);
                        break;

                    case Win32.SW_SHOWMAXIMIZED:
                        window.OsVisibility = Visibility.Maximised;
                        if (windowRect.HasValue)
                        {
                            window.RectangleCurrent = Win32Helper.GetRectangleFromRect(windowRect.Value);
                        }
                        window.Rectangle = Win32Helper.GetRectangleFromRect(windowPlacement.rcNormalPosition);
                        break;

                    default:
                        if (isVisible)
                        {
                            window.OsVisibility = Visibility.Normal;
                            if (windowRect.HasValue)
                            {
                                window.RectangleCurrent = Win32Helper.GetRectangleFromRect(windowRect.Value);
                            }
                            window.Rectangle = Win32Helper.GetRectangleFromRect(windowPlacement.rcNormalPosition);
                        }
                        else
                        {
                            window.OsVisibility = Visibility.None;
                            window.Rectangle = null;
                        }
                        break;
                }
            }

            if
                (
                    (window.Rectangle == null)
                    ||
                    (
                        (window.Rectangle != null)
                        &&
                        (
                            (window.Rectangle.Size.Width == 0)
                            ||
                            (window.Rectangle.Size.Height == 0)
                        )
                    )
                )
            {
                return;
            }

            windows[handle] = window;
        }

        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        public static IDictionary<IntPtr, IInternalWindow> GetOpenWindows()
        {
            IntPtr shellWindow = Win32.GetShellWindow();
            IntPtr desktopWindow = Win32.GetDesktopWindow();
            Dictionary<IntPtr, IInternalWindow> windows = new Dictionary<IntPtr, IInternalWindow>();

            // AddWindow(windows, desktopWindow, shellWindow);
            // AddWindow(windows, shellWindow, IntPtr.Zero);

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

            Win32.EnumWindows(delegate (IntPtr handle, int lParam)
            {
                windows.Add(handle);
                return true;
            }, 0);

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

        public static bool GetWindowStyles(IntPtr handle, out uint styles, out uint extStyles)
        {
            try
            {
                styles = unchecked((uint)Win32.GetWindowLong(handle, Win32.GWL_STYLE).ToInt32());
                extStyles = unchecked((uint)Win32.GetWindowLong(handle, Win32.GWL_EXSTYLE).ToInt32());
                return true;
            }
            catch
            {
                styles = 0;
                extStyles = 0;
                return false;
            }
        }

        public static bool SetWindowStyles(IntPtr handle, uint stylesToAdd, uint stylesToRemove, uint extStylesToAdd, uint extStyleToRemove)
        {
            try
            {
                var styles = unchecked((uint)Win32.GetWindowLong(handle, Win32.GWL_STYLE).ToInt32());
                var extStyles = unchecked((uint)Win32.GetWindowLong(handle, Win32.GWL_EXSTYLE).ToInt32());

                styles |= stylesToAdd;
                styles &= ~stylesToRemove;
                extStyles |= extStylesToAdd;
                extStyles &= ~extStyleToRemove;

                Win32.SetWindowLong(handle, Win32.GWL_STYLE, new IntPtr(styles));
                Win32.SetWindowLong(handle, Win32.GWL_EXSTYLE, new IntPtr(extStyles));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}