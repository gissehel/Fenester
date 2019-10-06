using Fenester.Lib.Win.Service.Helpers;
using System;

namespace Fenester.Lib.Win.Service
{
    public interface IMessageProcessor
    {
        IntPtr OnMessage(Message message);
    }
}