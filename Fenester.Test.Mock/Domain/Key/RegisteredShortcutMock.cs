using Fenester.Lib.Core.Domain.Fenester;
using Fenester.Lib.Core.Domain.Key;

namespace Fenester.Test.Mock.Domain.Key
{
    public class RegisteredShortcutMock : IRegisteredShortcut
    {
        public RegisteredShortcutMock(ShortcutMock shortcut, IOperation operation)
        {
            Shortcut = shortcut;
            Operation = operation;
        }

        public ShortcutMock Shortcut { get; }
        IShortcut IRegisteredShortcut.Shortcut => Shortcut;

        public IOperation Operation { get; }
    }
}