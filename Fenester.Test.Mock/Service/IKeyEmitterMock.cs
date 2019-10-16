using Fenester.Test.Mock.Domain.Key;
using System;

namespace Fenester.Test.Mock.Service
{
    public interface IKeyEmitterMock
    {
        Action<ShortcutMock> OnEmit { get; set; }
    }
}