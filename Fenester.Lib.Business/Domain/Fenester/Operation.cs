using Fenester.Lib.Core.Domain.Fenester;
using System;

namespace Fenester.Lib.Business.Domain.Fenester
{
    public class Operation : IOperation
    {
        public Operation(string name, Action action)
        {
            Name = name;
            Action = action;
        }

        public string Name { get; set; }
        public Action Action { get; set; }
    }
}