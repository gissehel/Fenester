using Fenester.Lib.Core.Domain.Key;
using Fenester.Lib.Core.Enums;

namespace Fenester.Test.Mock.Domain.Key
{
    public class KeyMock : IKey
    {
        public KeyMock(string name)
        {
            Name = name;
            Dead = false;
            KeyModifier = KeyModifier.None;
        }

        public KeyMock(string name, bool dead)
        {
            Name = name;
            Dead = dead;
            KeyModifier = KeyModifier.None;
        }

        public KeyMock(string name, KeyModifier keyModifier)
        {
            Name = name;
            Dead = true;
            KeyModifier = keyModifier;
        }

        public string Name { get; }

        public bool Dead { get; }

        public KeyModifier KeyModifier { get; }
    }
}