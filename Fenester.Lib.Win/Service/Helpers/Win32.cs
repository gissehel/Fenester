using Fenester.Lib.Win.Service.Helpers.Enums;
using Fenester.Lib.Win.Service.Helpers.Structs;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Fenester.Lib.Win.Service.Helpers
{
    public class Win32
    {
        #region Monitors

        [DllImport("user32.dll")]
        public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(IntPtr monitorHandle, ref MonitorInfo monitorInfo);

        #endregion Monitors

        #region Window

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr handle, out WindowPlacement windowPlacement);

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr handle, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr handle);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr handle, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr handle);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr handle, out Rect lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetShellWindow();

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr handle, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr parentHandle, Win32Callback callback, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetFocus(IntPtr handle);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr handle);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void SwitchToThisWindow(IntPtr handle, bool altTab);

        #endregion Window

        #region Window styles

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, GWL nIndex);

        public static IntPtr SetWindowLong(IntPtr handle, GWL nIndex, IntPtr newLong)
        {
            Error error = 0;
            IntPtr result = IntPtr.Zero;
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                var tempResult = IntSetWindowLong(handle, nIndex, ConvertHelper.ToInt32(newLong));
                error = GetLastError();
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(handle, nIndex, newLong);
                error = GetLastError();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception((int)error);
            }

            return result;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr handle, GWL nIndex, IntPtr newLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern int IntSetWindowLong(IntPtr handle, GWL nIndex, int newLong);

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int errorCode);

        [DllImport("user32.dll")]
        public static extern bool DrawMenuBar(IntPtr handle);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr handle, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        #endregion Window styles

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr handle, WM Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr handle, WM Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr handle, IntPtr ProcessId);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr handle, SW nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr handle);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr handle, IntPtr handleInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr handle, IntPtr lprcUpdate, IntPtr hrgnUpdate, RDW flags);

        [DllImport("coredll.dll")]
        public static extern void InvalidateRect(IntPtr handle, Rect rect, bool erase);

        #region keys

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr handle, int id, KeyModifiers fsModifiers, Keys vk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr handle, int id);

        #endregion keys

        #region window for keys

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "CreateWindowEx")]
        public static extern IntPtr CreateWindowEx
            (
                WS_EX exStyle,
                ushort registeredClassEx,
                string windowName,
                WS style,
                int x,
                int y,
                int nWidth,
                int nHeight,
                IntPtr handleWindowParent,
                IntPtr handleMenu,
                IntPtr handleInstance,
                IntPtr param
            );

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "RegisterClassEx")]
        public static extern ushort RegisterClassEx([In] ref WindowClassEx windowClassEx);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterClass(string className, IntPtr handleInstance);

        [DllImport("user32.dll")]
        public static extern int GetMessage(out Message message, IntPtr handle, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProc(IntPtr handleWindow, WM message, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern void PostQuitMessage(int exitCode);

        //[DllImport("user32.dll")]
        //static extern sbyte GetMessage(out Message lpMsg, IntPtr handleWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursor(IntPtr handleInstance, int cursorName);

        [DllImport("user32.dll")]
        public static extern bool TranslateMessage([In] ref Message message);

        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage([In] ref Message message);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyWindow(IntPtr handleWindow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern WAIT MsgWaitForMultipleObjectsEx(uint count, IntPtr[] pHandles, uint milliseconds, QS dwWakeMask, MWMO dwFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PeekMessage(out Message message, IntPtr handleWindow, uint wMsgFilterMin, uint wMsgFilterMax, PM wRemoveMsg);

        #endregion window for keys

        #region hooks

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr handleHook);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(WH hookType, HookProc func, IntPtr handleInstance, int threadID);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr handleHook, int code, IntPtr wParam, IntPtr lParam);

        #endregion hooks

        #region RawInput

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterRawInputDevices
        (
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] RawInputDevice[] rawInputDevices,
            int numDevices,
            int size
        );

        /// <summary>
        /// Function to retrieve raw input data.
        /// </summary>
        /// <param name="handleRawInput">Handle to the raw input.</param>
        /// <param name="command">Command to issue when retrieving data.</param>
        /// <param name="rawInput">Raw input data.</param>
        /// <param name="size">Number of bytes in the array.</param>
        /// <param name="sizeHeader">Size of the header.</param>
        /// <returns>0 if successful if rawInput is null, otherwise number of bytes if rawInput is not null.</returns>
        [DllImport("user32.dll")]
        public static extern int GetRawInputData(IntPtr handleRawInput, RawInputCommand command, out RawInput rawInput, ref int size, int sizeHeader);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string fileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string moduleName);

        #endregion RawInput

        #region Input

        /// <summary>
        /// Synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        [DllImport("user32.dll")]
        internal static extern uint SendInput
            (
                uint nInputs, 
                [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs,
                int cbSize
            );

        #endregion

        #region Errors

        public static Error GetLastError() => (Error)Marshal.GetLastWin32Error();

        #endregion Errors
    }
}