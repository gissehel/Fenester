using Fenester.Lib.Core.Service;
using Orissev.Win32.Structs;
using System;

namespace Fenester.Lib.Win.Service
{
    public interface IRunServiceWin : IRunService
    {
        IntPtr Handle { get; }

        void AddFuncMessageProcessor(Func<Message, IntPtr> funcMessageProcessor);

        void RegisterMessageProcessor(IMessageProcessor messageProcessor);

        void UnregisterMessageProcessor(IMessageProcessor messageProcessor);
    }
}