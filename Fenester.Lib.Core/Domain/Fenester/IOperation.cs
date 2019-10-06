using System;

namespace Fenester.Lib.Core.Domain.Fenester
{
    public interface IOperation
    {
        string Name { get; }

        Action Action { get; }
    }
}