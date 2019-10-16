using Fenester.Lib.Core.Enums;
using Fenester.Test.Mock.Domain.Key;

namespace Fenester.Test.Mock.Service
{
    public class KeysMock
    {
        public KeyMock Up { get; } = new KeyMock("Up");
        public KeyMock Down { get; } = new KeyMock("Down");
        public KeyMock Left { get; } = new KeyMock("Left");
        public KeyMock Right { get; } = new KeyMock("Right");
        public KeyMock Ctrl { get; } = new KeyMock("Ctrl", KeyModifier.Ctrl);
        public KeyMock Alt { get; } = new KeyMock("Alt", KeyModifier.Alt);
        public KeyMock Win { get; } = new KeyMock("Win", KeyModifier.Win);
        public KeyMock Shift { get; } = new KeyMock("Shift", KeyModifier.Shift);
        public KeyMock A { get; } = new KeyMock("A");
        public KeyMock B { get; } = new KeyMock("B");
        public KeyMock C { get; } = new KeyMock("C");
        public KeyMock D { get; } = new KeyMock("D");
        public KeyMock E { get; } = new KeyMock("E");
        public KeyMock F { get; } = new KeyMock("F");
        public KeyMock G { get; } = new KeyMock("G");
        public KeyMock H { get; } = new KeyMock("H");
        public KeyMock I { get; } = new KeyMock("I");
        public KeyMock J { get; } = new KeyMock("J");
        public KeyMock K { get; } = new KeyMock("K");
        public KeyMock L { get; } = new KeyMock("L");
        public KeyMock M { get; } = new KeyMock("M");
        public KeyMock N { get; } = new KeyMock("N");
        public KeyMock O { get; } = new KeyMock("O");
        public KeyMock P { get; } = new KeyMock("P");
        public KeyMock Q { get; } = new KeyMock("Q");
        public KeyMock R { get; } = new KeyMock("R");
        public KeyMock S { get; } = new KeyMock("S");
        public KeyMock T { get; } = new KeyMock("T");
        public KeyMock U { get; } = new KeyMock("U");
        public KeyMock V { get; } = new KeyMock("V");
        public KeyMock W { get; } = new KeyMock("W");
        public KeyMock X { get; } = new KeyMock("X");
        public KeyMock Y { get; } = new KeyMock("Y");
        public KeyMock Z { get; } = new KeyMock("Z");
    }
}