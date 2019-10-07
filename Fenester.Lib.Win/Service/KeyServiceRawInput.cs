using Fenester.Lib.Core.Domain.Fenester;
using Fenester.Lib.Core.Domain.Key;
using Fenester.Lib.Core.Enums;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Win.Domain.Key;
using Fenester.Lib.Win.Service.Helpers;
using Fenester.Lib.Win.Service.Helpers.Enums;
using Fenester.Lib.Win.Service.Helpers.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service
{
    public class KeyServiceRawInput : IKeyService, ITracable
    {
        public KeyServiceRawInput()
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
                    this.LogLine("RegisterHotKey/RawInput({0}, {1}, {2})", id, operation.Name, shortcut.Name);
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
                this.LogLine("UnregisterHotKey/RawInput({0})", id);
            }
        }

        public string ClassName { get; set; }
        public IntPtr Handle { get; set; }

        public void Init()
        {
            this.LogLine("KeyServiceRawInput.Init()");

            ClassName = "Fenester::KeyServiceRawInput-" + Guid.NewGuid().ToString();
            IntPtr OnMessage(Message message)
            {
                this.LogLine("    => Local message : {0} - {1} - {2} - {3}", message.handle.ToRepr(), message.message.ToRepr(), message.wParam.ToString(), message.lParam.ToString());
                if (message.message == WM.INPUT)
                {
                    int size = Marshal.SizeOf(typeof(RawInput));
                    var sizeRead = Win32.GetRawInputData(message.lParam, RawInputCommand.Input, out RawInput rawInput, ref size, Marshal.SizeOf(typeof(RawInputHeader)));
                    this.LogLine("        => sizeRead : {0}", sizeRead);
                    if (sizeRead > 0)
                    {
                        this.LogLine("        => rawInput.Header.Type : {0}", rawInput.Header.Type.ToRepr());
                        if (rawInput.Header.Type == RawInputType.Keyboard)
                        {
                            this.LogLine("        => rawInput.Keyboard.MakeCode : {0}", rawInput.Keyboard.MakeCode);
                            this.LogLine("        => rawInput.Keyboard.Flags : {0}", rawInput.Keyboard.Flags.ToRepr());
                            this.LogLine("        => rawInput.Keyboard.Reserved : {0}", rawInput.Keyboard.Reserved);
                            this.LogLine("        => rawInput.Keyboard.ExtraInformation : {0}", rawInput.Keyboard.ExtraInformation);
                            this.LogLine("        => rawInput.Keyboard.Message : {0}", rawInput.Keyboard.Message.ToRepr());
                            this.LogLine("        => rawInput.Keyboard.VirtualKey : {0}", rawInput.Keyboard.VirtualKey.ToRepr());
                        }
                    }
                }
                return IntPtr.Zero;
            }
            Handle = Win32Window.CreateWindow(OnMessage, ClassName);

            var rawInputDevice = new RawInputDevice
            {
                UsagePage = HIDUsagePage.Generic,
                Usage = HIDUsage.Keyboard,
                Flags = RIDEV.INPUTSINK | RIDEV.DEVNOTIFY,
                WindowHandle = Handle,
            };
            var result = Win32.RegisterRawInputDevices
            (
                new RawInputDevice[] { rawInputDevice },
                1,
                Marshal.SizeOf(typeof(RawInputDevice))
            );
            if (!result)
            {
                var error = Win32.GetLastError();
                this.LogLine("    => Error : {0}", error.ToRepr());
            }
            else
            {
                this.LogLine("    => Success");
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
            var rawInputDevice = new RawInputDevice
            {
                UsagePage = HIDUsagePage.Generic,
                Usage = HIDUsage.Keyboard,
                Flags = RIDEV.REMOVE,
                WindowHandle = IntPtr.Zero,
            };
            var result = Win32.RegisterRawInputDevices
            (
                new RawInputDevice[] { rawInputDevice },
                1,
                Marshal.SizeOf(typeof(RawInputDevice))
            );
            if (Handle != IntPtr.Zero)
            {
                Win32.DestroyWindow(Handle);
                Handle = IntPtr.Zero;
            }
            if (ClassName != null)
            {
                Win32.UnregisterClass(ClassName, IntPtr.Zero);
                ClassName = null;
            }
        }
    }
}