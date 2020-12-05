using Fenester.Lib.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Fenester.Lib.Core.Domain.Utils
{
    public class ComponentExpressions : List<Expression<Func<IComponent>>> { }
}
