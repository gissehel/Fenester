using Orissev.Win32.Structs;
using System;

namespace Fenester.Lib.Win.Service
{
    public interface IMessageProcessor
    {
        IntPtr OnMessage(Message message);
    }
}