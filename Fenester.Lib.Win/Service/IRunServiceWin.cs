using Fenester.Lib.Core.Service;
using System;

namespace Fenester.Lib.Win.Service
{
    public interface IRunServiceWin : IRunService
    {
        IntPtr Handle { get; }

        void RegisterMessageProcessor(IMessageProcessor messageProcessor);

        void UnregisterMessageProcessor(IMessageProcessor messageProcessor);
    }
}