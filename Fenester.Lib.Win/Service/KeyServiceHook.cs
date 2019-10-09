using Fenester.Lib.Core.Service;
using Fenester.Lib.Win.Service.Helpers;
using System;
using System.Diagnostics;
using System.Linq;

namespace Fenester.Lib.Win.Service
{
    public class KeyServiceHook : KeyServiceBase<Keys>
    {
        public KeyServiceHook()
        {
        }

        public override void Init()
        {
            this.LogLine("KeyServiceHook.Init()");

            hookProc = OnKeyboard;

            // InstallHook(Win32.LoadLibrary("User32"));
            // InstallHook(Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]));
            // InstallHook(IntPtr.Zero);

            using (Process process = Process.GetCurrentProcess())
            {
                using (ProcessModule module = process.MainModule)
                {
                    InstallHook(Win32.GetModuleHandle(module.ModuleName));
                }
            }
        }

        private HookProc hookProc = null;
        private IntPtr HandleHook { get; set; }

        private void InstallHook(IntPtr handleInstance)
        {
            this.LogLine("handleInstance : {0}", handleInstance.ToRepr());
            HandleHook = Win32.SetWindowsHookEx(WH.KEYBOARD_LL, hookProc, handleInstance, 0);
            if (HandleHook == IntPtr.Zero)
            {
                var error = Win32.GetLastError();
                this.LogLine("    => Error : {0}", error.ToRepr());
            }
            else
            {
                this.LogLine("    => {0}", HandleHook.ToRepr());
            }
        }

        public IntPtr OnKeyboard(int code, IntPtr wParam, IntPtr lParam)
        {
            this.LogLine("OnKeyboard({0}, {1}, {2})", code, wParam.ToInt32(), lParam.ToInt32());

            return Win32.CallNextHookEx(HandleHook, code, wParam, lParam);
        }

        public override void Uninit()
        {
            this.LogLine("KeyServiceHook.Uninit()");
            foreach (var registeredShortcut in RegisteredShortcuts.Values.ToList())
            {
                UnregisterShortcut(registeredShortcut);
            }
            RegisteredShortcuts.Clear();
            if (HandleHook != IntPtr.Zero)
            {
                var result = Win32.UnhookWindowsHookEx(HandleHook);
                this.LogLine("UnhookWindowsHookEx({0}) => {1}", HandleHook.ToRepr(), result.ToRepr());
                HandleHook = IntPtr.Zero;
            }
        }
    }
}