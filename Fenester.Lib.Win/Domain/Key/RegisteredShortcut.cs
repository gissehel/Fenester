using Fenester.Lib.Core.Domain.Fenester;
using Fenester.Lib.Core.Domain.Key;
using System;

namespace Fenester.Lib.Win.Domain.Key
{
    public class RegisteredShortcut<E> : IRegisteredShortcut where E : struct, IComparable
    {
        public RegisteredShortcut(Shortcut<E> shortcut, IOperation operation, int id)
        {
            Id = id;
            Shortcut = shortcut;
            Operation = operation;
        }

        public int Id { get; set; }

        public Shortcut<E> Shortcut { get; }

        public IOperation Operation { get; }

        IShortcut IRegisteredShortcut.Shortcut => Shortcut;
    }
}