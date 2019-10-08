using Fenester.Lib.Core.Domain.Key;
using System;

namespace Fenester.Lib.Win.Domain.Key
{
    public class Key<E> : IKey where E : struct, IComparable
    {
        public Key(E e)
        {
            Value = e;

            Name = e.ToString();
        }

        public string Name { get; private set; }

        public E Value { get; }
    }
}