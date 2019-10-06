using Fenester.Lib.Core.Service;
using Fenester.Lib.Win.Enums;
using Fenester.Lib.Win.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fenester.Lib.Win.Service
{
    public class RunService : IRunService, IMessageProcessor, ITracable
    {
        public IntPtr Handle { get; set; }

        private string ClassName { get; set; }

        private bool UseWindow { get; } = false;

        private List<IMessageProcessor> MessageProcessors { get; } = new List<IMessageProcessor>();

        private List<Func<Message, IntPtr>> FuncMessageProcessors { get; } = new List<Func<Message, IntPtr>>();

        private IEnumerable<Func<Message, IntPtr>> AllMessageProcessors
            => MessageProcessors
                .Select<IMessageProcessor, Func<Message, IntPtr>>(mp => mp.OnMessage)
                .Concat(FuncMessageProcessors);

        public Action<string> OnLogLine { get; set; }

        public void Init()
        {
            if (UseWindow)
            {
                ClassName = "Fenester::KeyService-" + Guid.NewGuid().ToString();
                Handle = Win32Window.CreateWindow(OnMessage, ClassName);
            }
            else
            {
                Handle = IntPtr.Zero;
            }

            this.LogLine("RunService.Init : ({0}) => {1}", ClassName, Handle.ToRepr());
            if (Handle != IntPtr.Zero)
            {
                Win32.ShowWindow(Handle, SW.MINIMIZE);
            }
        }

        public void Uninit()
        {
            this.LogLine("RunService.Uninit : {0}", Handle.ToRepr());
            if (Handle != IntPtr.Zero)
            {
                Win32.DestroyWindow(Handle);
                Win32.UnregisterClass(ClassName, IntPtr.Zero);
                Handle = IntPtr.Zero;
            }
        }

        public void RegisterMessageProcessor(IMessageProcessor messageProcessor)
        {
            if (!MessageProcessors.Contains(messageProcessor))
            {
                MessageProcessors.Add(messageProcessor);
            }
        }

        public void UnregisterMessageProcessor(IMessageProcessor messageProcessor)
        {
            if (MessageProcessors.Contains(messageProcessor))
            {
                MessageProcessors.Remove(messageProcessor);
            }
        }

        public void Run()
        {
            bool continueRun = true;
            Func<Message, IntPtr> onMessage = (message) =>
            {
                if (message.message == UserMessage.Quit.ToWM())
                {
                    continueRun = false;
                }
                return IntPtr.Zero;
            };
            FuncMessageProcessors.Add(onMessage);
            Win32Window.ListenMessages(Handle, TimeSpan.Zero, OnMessage, () => continueRun);
            FuncMessageProcessors.Remove(onMessage);
        }

        public void RunFor(TimeSpan timeSpan)
        {
            Win32Window.ListenMessages(Handle, timeSpan, OnMessage, null);
        }

        public void Stop()
        {
            Win32.PostMessage(Handle, UserMessage.Quit.ToWM(), 0, 0);
        }

        public IntPtr OnMessage(Message message)
        {
            bool messageProcessed = false;
            IntPtr result = IntPtr.Zero;
            foreach (var messageProcessors in AllMessageProcessors)
            {
                if (!messageProcessed)
                {
                    IntPtr localResult = messageProcessors(message);
                    if (localResult != IntPtr.Zero)
                    {
                        messageProcessed = true;
                        result = localResult;
                    }
                }
            }
            return result;
        }
    }
}