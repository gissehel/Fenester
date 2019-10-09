using Fenester.Lib.Core.Domain.Fenester;
using Fenester.Lib.Core.Domain.Key;
using Fenester.Lib.Core.Enums;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Win.Domain.Key;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fenester.Lib.Win.Service
{
    public abstract class KeyServiceBase<E> : IKeyService, ITracable where E : struct, IComparable
    {
        protected List<Key<E>> Keys { get; set; } = Enum
            .GetValues(typeof(E))
            .Cast<E>()
            .Select(e => new Key<E>(e))
            .ToList()
        ;

        public Action<string> OnLogLine { get; set; }

        public IEnumerable<IKey> GetKeys() => Keys.Cast<IKey>();

        public IShortcut GetShortcut(IKey iKey, KeyModifier keyModifier)
        {
            if (iKey is Key<E> key)
            {
                return new Shortcut<E>(key, keyModifier);
            }
            return null;
        }

        private int NextIdToRegister { get; set; } = 1;

        protected Dictionary<int, RegisteredShortcut<E>> RegisteredShortcuts { get; } = new Dictionary<int, RegisteredShortcut<E>>();

        protected virtual bool RegisterHotKey(Shortcut<E> shortcut, int id) => true;

        protected virtual bool UnregisterHotKey(Shortcut<E> shortcut, int id) => true;

        public IRegisteredShortcut RegisterShortcut(IShortcut iShortcut, IOperation operation)
        {
            if (iShortcut is Shortcut<E> shortcut)
            {
                try
                {
                    int id = NextIdToRegister;
                    NextIdToRegister += 1;
                    var result = RegisterHotKey(shortcut, id);
                    this.LogLine("RegisterHotKey/{3}({0}, {1}, {2}) => {4}", id, operation.Name, shortcut.Name, GetType().Name, result.ToRepr());
                    if (result)
                    {
                        var registeredShortcut = new RegisteredShortcut<E>(shortcut, operation, id);
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
            if (iRegisteredShortcut is RegisteredShortcut<E> registeredShortcut)
            {
                var id = registeredShortcut.Id;
                if (RegisteredShortcuts.ContainsKey(id))
                {
                    RegisteredShortcuts.Remove(id);
                }
                UnregisterHotKey(registeredShortcut.Shortcut, id);
                this.LogLine("UnregisterHotKey/{1}({0})", id, GetType().Name);
            }
        }

        protected void ExecuteRegisteredShortcut(RegisteredShortcut<E> registeredShortcut)
        {
            this.LogLine("  Executing action [{0}] due to shortcut [{1}] registered as [{2}]", registeredShortcut.Operation.Name, registeredShortcut.Shortcut.Name, registeredShortcut.Id);
            registeredShortcut.Operation.Action();
        }

        public abstract void Init();

        public abstract void Uninit();
    }
}