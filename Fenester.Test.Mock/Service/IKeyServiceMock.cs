using Fenester.Lib.Core.Domain.Fenester;
using Fenester.Lib.Core.Domain.Key;
using System;

namespace Fenester.Test.Mock.Service
{
    public interface IKeyServiceMock : IInitializableMock, IKeyEmitterMock
    {
        Action<IRegisteredShortcut> OnUnregisterShortcut { get; set; }

        Func<IShortcut, IOperation, IRegisteredShortcut> OnRegisterShortcut { get; set; }

        KeysMock Keys { get; }
    }
}