using Fenester.Lib.Core.Enums;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Win.Domain.Key;
using Orissev.Win32;
using Orissev.Win32.Enums;
using Orissev.Win32.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Fenester.Lib.Win.Service
{
    public class KeyServiceRawInput : KeyServiceBase<VirtualKeys>, IMessageProcessor
    {
        private IRunServiceWin RunService { get; set; }

        public KeyServiceRawInput(IRunServiceWin runService)
        {
            RunService = runService;
        }

        public override void Init()
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

        public override void Uninit()
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

        private HashSet<VirtualKeys> KeyUsed { get; } = new HashSet<VirtualKeys>();

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

        private bool AddKey(VirtualKeys virtualKey)
        {
            KeyPressed.Add(virtualKey);
            bool result = false;
            this.LogLine("        => Keyboard state : {0}", string.Join(",", KeyPressed.Select(x => x.ToEnumName())));
            foreach (var registeredShortcut in RegisteredShortcuts.Values)
            {
                if (KeyPressedMatchShortcut(registeredShortcut.Shortcut))
                {
                    KeyUsed.Add(virtualKey);
                    result = true;
                    ExecuteRegisteredShortcut(registeredShortcut);
                }
            }
            return result;
        }

        private bool RemoveKey(VirtualKeys virtualKey)
        {
            bool result = false;
            if (!KeyPressed.Contains(virtualKey))
            {
                AddKey(virtualKey);
            }
            KeyPressed.Remove(virtualKey);
            if (KeyUsed.Contains(virtualKey))
            {
                KeyUsed.Remove(virtualKey);
                result = true;
            }
            return result;
        }

        public IntPtr OnMessage(Message message)
        {
            bool handled = false;
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
                            handled = handled || AddKey(rawInput.Keyboard.VirtualKey);
                        }
                        if (rawInput.Keyboard.Message == WM.KEYUP || rawInput.Keyboard.Message == WM.SYSKEYUP)
                        {
                            handled = handled || RemoveKey(rawInput.Keyboard.VirtualKey);
                        }
                    }
                }
            }
            return handled ? (IntPtr)(1) : IntPtr.Zero;
        }
    }
}