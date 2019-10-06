using Fenester.Lib.Core.Domain.Fenester;
using Fenester.Lib.Core.Domain.Key;
using Fenester.Lib.Core.Enums;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Win.Domain.Key;
using Fenester.Lib.Win.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fenester.Lib.Win.Service
{
    public class KeyService : IKeyService, IMessageProcessor, ITracable
    {
        private int NextIdToRegister { get; set; } = 1;

        private IRunServiceWin RunService { get; set; }

        private IntPtr Handle => RunService.Handle;

        public Action<string> OnLogLine { get; set; }

        public KeyService(IRunServiceWin runService)
        {
            RunService = runService;
        }

        public void Init()
        {
            this.LogLine("KeyService.Init()");
            RunService.RegisterMessageProcessor(this);
        }

        public void Uninit()
        {
            this.LogLine("KeyService.Uninit()");
            RunService.UnregisterMessageProcessor(this);
            foreach (var registeredShortcut in RegisteredShortcuts.Values.ToList())
            {
                UnregisterShortcut(registeredShortcut);
            }
            RegisteredShortcuts.Clear();
        }

        public IntPtr OnMessage(Message message)
        {
            this.LogLine("Message on {0} : {1}", message.handle.ToRepr(), message.message.ToRepr());
            switch (message.message)
            {
                case WM.HOTKEY:
                    int id = message.wParam.ToInt32();
                    if (RegisteredShortcuts.ContainsKey(id))
                    {
                        var registeredShortcut = RegisteredShortcuts[id];
                        this.LogLine("  Executing action [{0}] due to shortcut [{1}] registered as [{2}]", registeredShortcut.Operation.Name, registeredShortcut.Shortcut.Name, registeredShortcut.Id);
                        registeredShortcut.Operation.Action();
                    }
                    break;

                default:
                    break;
            }

            return IntPtr.Zero;
        }

        private List<Key> Keys { get; set; } = Enum
            .GetValues(typeof(Keys))
            .Cast<Keys>()
            .Select(keys => new Key(keys))
            .ToList()
        ;

        private Dictionary<int, RegisteredShortcut> RegisteredShortcuts { get; } = new Dictionary<int, RegisteredShortcut>();

        public IEnumerable<IKey> GetKeys() => Keys.Cast<IKey>();

        public IShortcut GetShortcut(IKey iKey, KeyModifier keyModifier)
        {
            if (iKey is Key key)
            {
                return new Shortcut(key, keyModifier);
            }
            return null;
        }

        public IRegisteredShortcut RegisterShortcut(IShortcut iShortcut, IOperation operation)
        {
            if (iShortcut is Shortcut shortcut)
            {
                try
                {
                    int id = NextIdToRegister;
                    NextIdToRegister += 1;
                    var result = Win32.RegisterHotKey
                        (
                            Handle,
                            id,
                            shortcut.KeyModifier.ToKeyModifiers(),
                            shortcut.Key.Keys
                        );
                    this.LogLine("RegisterHotKey({0}, {1}, {2}, {3}) => [{4}]", Handle.ToRepr(), id, operation.Name, shortcut.Name, result ? "true" : "false");
                    if (result)
                    {
                        var registeredShortcut = new RegisteredShortcut(shortcut, operation, id);
                        RegisteredShortcuts[id] = registeredShortcut;
                        return registeredShortcut;
                    }
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
                this.LogLine("UnregisterHotKey({0}, {1})", Handle.ToRepr(), id);
                Win32.UnregisterHotKey(Handle, id);
            }
        }
    }
}