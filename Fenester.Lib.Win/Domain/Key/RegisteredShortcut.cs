using Fenester.Lib.Core.Domain.Fenester;
using Fenester.Lib.Core.Domain.Key;

namespace Fenester.Lib.Win.Domain.Key
{
    public class RegisteredShortcut : IRegisteredShortcut
    {
        public RegisteredShortcut(Shortcut shortcut, IOperation operation, int id)
        {
            Id = id;
            Shortcut = shortcut;
            Operation = operation;
        }

        public int Id { get; set; }

        public Shortcut Shortcut { get; }

        public IOperation Operation { get; }

        IShortcut IRegisteredShortcut.Shortcut => Shortcut;
    }
}