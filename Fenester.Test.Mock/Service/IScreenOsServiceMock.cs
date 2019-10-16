using Fenester.Lib.Core.Domain.Os;
using Fenester.Test.Mock.Domain.Os;
using System;
using System.Collections.Generic;

namespace Fenester.Test.Mock.Service
{
    public interface IScreenOsServiceMock : IInitializableMock
    {
        Func<IEnumerable<IInternalScreen>> OnGetScreens { get; set; }

        List<InternalScreenMock> Screens { get; }

        void Add(InternalScreenMock internalScreen);
    }
}