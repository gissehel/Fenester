using Fenester.Lib.Core.Domain.Key;
using Fenester.Lib.Core.Enums;
using System;
using System.Linq;

namespace Fenester.Test.Mock.Domain.Key
{
    public class ShortcutMock : IShortcut
    {
        public ShortcutMock(KeyMock key, KeyModifier keyModifier)
        {
            Key = key;
            KeyModifier = keyModifier;
        }

        private static KeyModifier[] KeyModifiers { get; } = new KeyModifier[] { KeyModifier.Ctrl, KeyModifier.Win, KeyModifier.Alt, KeyModifier.Shift };

        public KeyModifier[] BaseModifiers => KeyModifiers.Where(mod => (mod & KeyModifier) != 0).ToArray();

        private string KeyModifierString
            => string.Join
            (
                "",
                BaseModifiers
                    .Select(mod => string.Format("{0}+", Enum.GetName(typeof(KeyModifier), mod)))
            );

        public string Name => string.Format("{0}{1}", KeyModifierString, Key.Name);

        public KeyMock Key { get; }
        IKey IShortcut.Key => Key;

        public KeyModifier KeyModifier { get; set; }

        private void SetKeyModifier(bool value, KeyModifier keyModifier)
        {
            if (value)
            {
                KeyModifier = KeyModifier | keyModifier;
            }
            else
            {
                KeyModifier = KeyModifier & (~keyModifier);
            }
        }

        public bool Ctrl
        {
            get => (KeyModifier & KeyModifier.Ctrl) != 0;
            set => SetKeyModifier(value, KeyModifier.Ctrl);
        }

        public bool Win
        {
            get => (KeyModifier & KeyModifier.Win) != 0;
            set => SetKeyModifier(value, KeyModifier.Win);
        }

        public bool Alt
        {
            get => (KeyModifier & KeyModifier.Alt) != 0;
            set => SetKeyModifier(value, KeyModifier.Alt);
        }

        public bool Shift
        {
            get => (KeyModifier & KeyModifier.Shift) != 0;
            set => SetKeyModifier(value, KeyModifier.Shift);
        }
    }
}