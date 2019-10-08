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
    public class KeyServiceRawInput : IKeyService, IMessageProcessor, ITracable
    {
        private IRunServiceWin RunService { get; set; }

        public KeyServiceRawInput(IRunServiceWin runService)
        {
            RunService = runService;
        }

        private List<Key<VirtualKeys>> Keys { get; set; } = Enum
            .GetValues(typeof(VirtualKeys))
            .Cast<VirtualKeys>()
            .Select(e => new Key<VirtualKeys>(e))
            .ToList()
        ;

        public Action<string> OnLogLine { get; set; }

        public IEnumerable<IKey> GetKeys() => Keys.Cast<IKey>();

        public IShortcut GetShortcut(IKey iKey, KeyModifier keyModifier)
        {
            if (iKey is Key<VirtualKeys> key)
            {
                return new Shortcut<VirtualKeys>(key, keyModifier);
            }
            return null;
        }

        private int NextIdToRegister { get; set; } = 1;
        private Dictionary<int, RegisteredShortcut<VirtualKeys>> RegisteredShortcuts { get; } = new Dictionary<int, RegisteredShortcut<VirtualKeys>>();

        public IRegisteredShortcut RegisterShortcut(IShortcut iShortcut, IOperation operation)
        {
            if (iShortcut is Shortcut<VirtualKeys> shortcut)
            {
                try
                {
                    int id = NextIdToRegister;
                    NextIdToRegister += 1;
                    this.LogLine("RegisterHotKey/RawInput({0}, {1}, {2})", id, operation.Name, shortcut.Name);
                    var registeredShortcut = new RegisteredShortcut<VirtualKeys>(shortcut, operation, id);
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
            if (iRegisteredShortcut is RegisteredShortcut<VirtualKeys> registeredShortcut)
            {
                var id = registeredShortcut.Id;
                if (RegisteredShortcuts.ContainsKey(id))
                {
                    RegisteredShortcuts.Remove(id);
                }
                this.LogLine("UnregisterHotKey/RawInput({0})", id);
            }
        }

        public void Init()
        {
            this.LogLine("KeyServiceRawInput.Init()");

            var rawInputDevice = new RawInputDevice
            {
                UsagePage = HIDUsagePage.Generic,
                Usage = HIDUsage.Keyboard,
                Flags = RIDEV.INPUTSINK | RIDEV.DEVNOTIFY,
                WindowHandle = RunService.Handle,
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

            RunService.RegisterMessageProcessor(this);
        }

        private IntPtr HandleHook { get; set; }

        public IntPtr OnKeyboard(int code, IntPtr wParam, IntPtr lParam)
        {
            this.LogLine("OnKeyboard({0}, {1}, {2})", code, wParam.ToInt32(), lParam.ToInt32());

            return Win32.CallNextHookEx(HandleHook, code, wParam, lParam);
        }

        public void Uninit()
        {
            this.LogLine("KeyServiceRawInput.Uninit()");
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
            RunService.UnregisterMessageProcessor(this);
        }

        private HashSet<VirtualKeys> KeyPressed { get; } = new HashSet<VirtualKeys>();

        private bool KeyPressedMatchShortcut(Shortcut<VirtualKeys> shortcut)
        {
            if (KeyPressed.Contains(shortcut.Key.Value))
            {
                if (shortcut.BaseModifiers.Length + 1 == KeyPressed.Count)
                {
                    bool allModifiers = true;
                    foreach (var baseModifier in shortcut.BaseModifiers)
                    {
                        switch (baseModifier)
                        {
                            case KeyModifier.Ctrl:
                                if (!KeyPressed.Contains(VirtualKeys.Control))
                                {
                                    allModifiers = false;
                                }
                                break;

                            case KeyModifier.Shift:
                                if (!KeyPressed.Contains(VirtualKeys.Shift))
                                {
                                    allModifiers = false;
                                }
                                break;

                            case KeyModifier.Alt:
                                if (!KeyPressed.Contains(VirtualKeys.Menu))
                                {
                                    allModifiers = false;
                                }
                                break;

                            case KeyModifier.Win:
                                if (!KeyPressed.Contains(VirtualKeys.LeftWindows))
                                {
                                    allModifiers = false;
                                }
                                break;

                            default:
                                allModifiers = false;
                                break;
                        }
                    }

                    if (allModifiers)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void AddKey(VirtualKeys virtualKey)
        {
            KeyPressed.Add(virtualKey);
            this.LogLine("        => Keyboard state : {0}", string.Join(",", KeyPressed.Select(x => x.ToEnumName())));
            foreach (var registeredShortcut in RegisteredShortcuts.Values)
            {
                if (KeyPressedMatchShortcut(registeredShortcut.Shortcut))
                {
                    this.LogLine("  Executing action [{0}] due to shortcut [{1}] registered as [{2}]", registeredShortcut.Operation.Name, registeredShortcut.Shortcut.Name, registeredShortcut.Id);
                    registeredShortcut.Operation.Action();
                }
            }
        }

        private void RemoveKey(VirtualKeys virtualKey)
        {
            if (!KeyPressed.Contains(virtualKey))
            {
                AddKey(virtualKey);
            }
            KeyPressed.Remove(virtualKey);
        }

        public IntPtr OnMessage(Message message)
        {
            // this.LogLine("    => Local message : {0} - {1} - {2} - {3}", message.handle.ToRepr(), message.message.ToRepr(), message.wParam.ToString(), message.lParam.ToString());
            if (message.message == WM.INPUT)
            {
                int size = Marshal.SizeOf(typeof(RawInput));
                var sizeRead = Win32.GetRawInputData(message.lParam, RawInputCommand.Input, out RawInput rawInput, ref size, Marshal.SizeOf(typeof(RawInputHeader)));
                // this.LogLine("        => sizeRead : {0}", sizeRead);
                if (sizeRead > 0)
                {
                    // this.LogLine("        => rawInput.Header.Type : {0}", rawInput.Header.Type.ToRepr());
                    if (rawInput.Header.Type == RawInputType.Keyboard)
                    {
                        // this.LogLine("        => rawInput.Keyboard.MakeCode : {0}", rawInput.Keyboard.MakeCode);
                        // this.LogLine("        => rawInput.Keyboard.Flags : {0}", rawInput.Keyboard.Flags.ToRepr());
                        // this.LogLine("        => rawInput.Keyboard.Reserved : {0}", rawInput.Keyboard.Reserved);
                        // this.LogLine("        => rawInput.Keyboard.ExtraInformation : {0}", rawInput.Keyboard.ExtraInformation);
                        // this.LogLine("        => rawInput.Keyboard.Message : {0}", rawInput.Keyboard.Message.ToRepr());
                        // this.LogLine("        => rawInput.Keyboard.VirtualKey : {0}", rawInput.Keyboard.VirtualKey.ToRepr());
                        if (rawInput.Keyboard.Message == WM.KEYDOWN || rawInput.Keyboard.Message == WM.SYSKEYDOWN)
                        {
                            AddKey(rawInput.Keyboard.VirtualKey);
                        }
                        if (rawInput.Keyboard.Message == WM.KEYUP || rawInput.Keyboard.Message == WM.SYSKEYUP)
                        {
                            RemoveKey(rawInput.Keyboard.VirtualKey);
                        }
                    }
                }
            }
            return IntPtr.Zero;
        }
    }
}