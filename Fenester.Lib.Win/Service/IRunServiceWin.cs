using Fenester.Lib.Core.Service;
using Fenester.Lib.Win.Service.Helpers;
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