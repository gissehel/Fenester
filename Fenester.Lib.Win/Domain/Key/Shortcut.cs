using Fenester.Lib.Core.Domain.Key;
using Fenester.Lib.Core.Enums;
using System;
using System.Linq;

namespace Fenester.Lib.Win.Domain.Key
{
    public class Shortcut : IShortcut
    {
        public Shortcut(Key key, KeyModifier keyModifier)
        {
            Key = key;
            KeyModifier = keyModifier;
        }

        private static KeyModifier[] KeyModifiers { get; } = new KeyModifier[] { KeyModifier.Ctrl, KeyModifier.Win, KeyModifier.Alt, KeyModifier.Shift };

        private string KeyModifierString
            => string.Join
            (
                "",
                KeyModifiers
                    .Where(mod => (mod & KeyModifier) != 0)
                    .Select(mod => string.Format("{0}+", Enum.GetName(typeof(KeyModifier), mod)))
            );

        public string Name => string.Format("{0}{1}", KeyModifierString, Key.Name);

        public Key Key { get; }

        public KeyModifier KeyModifier { get; set; }

        IKey IShortcut.Key => Key;

        public bool Ctrl
        {
            get => (KeyModifier & KeyModifier.Ctrl) != 0;
            set
            {
                if (value)
                {
                    KeyModifier = KeyModifier | KeyModifier.Ctrl;
                }
                else
                {
                    KeyModifier = KeyModifier & (~KeyModifier.Ctrl);
                }
            }
        }

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