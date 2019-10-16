using Fenester.Lib.Core.Domain.Fenester;
using Fenester.Lib.Core.Domain.Key;
using Fenester.Lib.Core.Enums;
using Fenester.Lib.Core.Service;
using Fenester.Test.Mock.Domain.Key;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fenester.Test.Mock.Service
{
    public class KeyServiceMock : IKeyService, IKeyEmitter, IKeyServiceMock, IKeyEmitterMock
    {
        public KeyServiceMock()
        {
            Keys = new KeysMock();
            KeyList = new List<KeyMock>
            {
                Keys.Up,
                Keys.Down,
                Keys.Left,
                Keys.Right,
                Keys.Ctrl,
                Keys.Alt,
                Keys.Win,
                Keys.Shift,
                Keys.A,
                Keys.B,
                Keys.C,
                Keys.D,
                Keys.E,
                Keys.F,
                Keys.G,
                Keys.H,
                Keys.J,
                Keys.L,
                Keys.M,
                Keys.N,
                Keys.O,
                Keys.P,
                Keys.Q,
                Keys.R,
                Keys.S,
                Keys.T,
                Keys.U,
                Keys.V,
                Keys.W,
                Keys.X,
                Keys.Y,
                Keys.Z,
            };
        }

        public KeysMock Keys { get; }

        public List<KeyMock> KeyList { get; }

        public IEnumerable<IKey> GetKeys() => KeyList;

        public IShortcut GetShortcut(IKey iKey, KeyModifier keyModifier)
        {
            if (iKey is KeyMock key)
            {
                return new ShortcutMock(key, keyModifier);
            }
            return null;
        }

        public Func<IShortcut, IOperation, IRegisteredShortcut> OnRegisterShortcut { get; set; }

        public IRegisteredShortcut RegisterShortcut(IShortcut iShortcut, IOperation operation)
        {
            if (OnRegisterShortcut == null)
            {
                if (iShortcut is ShortcutMock shortcut)
                {
                    if (!RegisteredShortcuts.ContainsKey(shortcut.Key.Name))
                    {
                        RegisteredShortcuts[shortcut.Key.Name] = new Dictionary<KeyModifier, RegisteredShortcutMock>();
                    }
                    if (!RegisteredShortcuts[shortcut.Key.Name].ContainsKey(shortcut.KeyModifier))
                    {
                        var registeredShortcut = new RegisteredShortcutMock(shortcut, operation);
                        RegisteredShortcuts[shortcut.Key.Name][shortcut.KeyModifier] = registeredShortcut;
                        return registeredShortcut;
                    }
                    return null;
                }
                throw new NotImplementedException();
            }
            else
            {
                return OnRegisterShortcut(iShortcut, operation);
            }
        }

        public Action<IRegisteredShortcut> OnUnregisterShortcut { get; set; }

        public void UnregisterShortcut(IRegisteredShortcut registeredShortcut)
        {
            if (OnUnregisterShortcut == null)
            {
                var shortcut = registeredShortcut.Shortcut;
                var key = shortcut.Key;
                var keyModifier = shortcut.KeyModifier;
                var name = key.Name;
                if (RegisteredShortcuts.ContainsKey(name))
                {
                    var registeredShortcutsForName = RegisteredShortcuts[name];
                    if (registeredShortcutsForName.ContainsKey(keyModifier))
                    {
                        if (registeredShortcutsForName[keyModifier] == registeredShortcut)
                        {
                            registeredShortcutsForName.Remove(keyModifier);
                        }
                        if (registeredShortcutsForName.Count == 0)
                        {
                            RegisteredShortcuts.Remove(name);
                        }
                    }
                }
            }
            else
            {
                OnUnregisterShortcut(registeredShortcut);
            }
        }

        public Action OnInit { get; set; }
        public Action OnUninit { get; set; }

        public void Init()
        {
            OnInit?.Invoke();
        }

        public void Uninit()
        {
            OnUninit?.Invoke();
        }

        public List<KeyMock> KeyPressed { get; } = new List<KeyMock>();

        public void Emit(KeyMock key)
        {
            KeyPressed.Add(key);
            if (!key.Dead)
            {
                Emit(KeyPressed);
                KeyPressed.Clear();
            }
        }

        private void Emit(List<KeyMock> keys)
        {
            var mainKey = keys.Where(key => key.KeyModifier == KeyModifier.None).Last();
            var keyModifier = keys
                .Where(key => key.KeyModifier != KeyModifier.None)
                .Select(key => key.KeyModifier)
                .Aggregate((k1, k2) => k1 | k2);

            var shortcut = new ShortcutMock(mainKey, keyModifier);
            var name = mainKey.Name;
            if (RegisteredShortcuts.ContainsKey(name))
            {
                var registeredShortcutsForKey = RegisteredShortcuts[name];
                if (registeredShortcutsForKey != null)
                {
                    if (registeredShortcutsForKey.ContainsKey(keyModifier))
                    {
                        var registeredShortcut = registeredShortcutsForKey[keyModifier];
                        if (registeredShortcut != null)
                        {
                            shortcut = registeredShortcut.Shortcut;
                            if (registeredShortcut.Operation != null)
                            {
                                registeredShortcut.Operation.Action();
                            }
                        }
                    }
                }
            }
            OnEmit?.Invoke(shortcut);
        }

        public Action<ShortcutMock> OnEmit { get; set; }

        public Dictionary<string, Dictionary<KeyModifier, RegisteredShortcutMock>> RegisteredShortcuts = new Dictionary<string, Dictionary<KeyModifier, RegisteredShortcutMock>>();
    }
}