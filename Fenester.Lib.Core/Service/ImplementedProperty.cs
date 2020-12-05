using System;
using System.Collections.Generic;
using System.Text;

namespace Fenester.Lib.Core.Service
{
    public class ImplementedProperty<IT, T> where T : IT
    {
        public IT Use => Impl;

        public T Impl { get; set; }

        public static implicit operator IT(ImplementedProperty<IT, T> self) => self.Impl;
    }
}
