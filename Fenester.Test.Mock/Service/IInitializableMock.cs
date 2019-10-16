using System;

namespace Fenester.Test.Mock.Service
{
    public interface IInitializableMock
    {
        Action OnInit { get; set; }

        Action OnUninit { get; set; }
    }
}