using Fenester.Lib.Core.Domain.Fenester;
using Fenester.Lib.Core.Domain.Key;
using Fenester.Lib.Core.Enums;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Win.Domain.Key;
using Fenester.Lib.Win.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service
{
    public class KeyServiceHook : IKeyService, ITracable
    {
        public KeyServiceHook()
        {
        }

        private List<Key> Keys { get; set; } = Enum
            .GetValues(typeof(Keys))
            .Cast<Keys>()
            .Select(keys => new Key(keys))
            .ToList()
        ;

        public Action<string> OnLogLine { get; set; }

        public IEnumerable<IKey> GetKeys() => Keys.Cast<IKey>();

        public IShortcut GetShortcut(IKey iKey, KeyModifier keyModifier)
        {
            if (iKey is Key key)
            {
                return new Shortcut(key, keyModifier);
            }
            return null;
        }

        private int NextIdToRegister { get; set; } = 1;
        private Dictionary<int, RegisteredShortcut> RegisteredShortcuts { get; } = new Dictionary<int, RegisteredShortcut>();

        public IRegisteredShortcut RegisterShortcut(IShortcut iShortcut, IOperation operation)
        {
            if (iShortcut is Shortcut shortcut)
            {
                try
                {
                    int id = NextIdToRegister;
                    NextIdToRegister += 1;
                    this.LogLine("RegisterHotKey/Hook({0}, {1}, {2})", id, operation.Name, shortcut.Name);
                    var registeredShortcut = new RegisteredShortcut(shortcut, operation, id);
                    RegisteredShortcuts[id] = registeredShortcut;
                    return registeredShortcut;
                }
                catch
                {
                }
            }
            return null;
        }

        public void UnregisterShortcut(IRegisteredShortcut iRegisteredShortcut)
        {
            if (iRegisteredShortcut is RegisteredShortcut registeredShortcut)
            {
                var id = registeredShortcut.Id;
                if (RegisteredShortcuts.ContainsKey(id))
                {
                    RegisteredShortcuts.Remove(id);
                }
                this.LogLine("UnregisterHotKey/Hook({0})", id);
            }
        }

        public void Init()
        {
            this.LogLine("KeyServiceHook.Init()");

            var handleInstance = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]);
            handleInstance = IntPtr.Zero;
            this.LogLine("handleInstance : {0}", handleInstance.ToRepr());
            HandleHook = Win32.SetWindowsHookEx(WH.KEYBOARD_LL, new HookProc(OnKeyboard), handleInstance, 0);
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

        private IntPtr HandleHook { get; set; }

        public IntPtr OnKeyboard(int code, IntPtr wParam, IntPtr lParam)
        {
            this.LogLine("OnKeyboard({0}, {1}, {2})", code, wParam.ToInt32(), lParam.ToInt32());

            return Win32.CallNextHookEx(HandleHook, code, wParam, lParam);
        }

        public void Uninit()
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