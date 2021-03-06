﻿using Fenester.Lib.Core.Service;
using Fenester.Lib.Win.Enums;
using Fenester.Lib.Win.Service.Helpers;
using Orissev.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Message = Orissev.Win32.Structs.Message;

namespace Fenester.Lib.Win.Service
{
    public class RunService : IRunServiceWin, IMessageProcessor, ITracable
    {
        public IntPtr Handle { get; set; }

        private string ClassName { get; set; }

        private RunWindowStrategy RunWindowStrategy { get; }

        private List<IMessageProcessor> MessageProcessors { get; } = new List<IMessageProcessor>();

        private List<Func<Message, IntPtr>> FuncMessageProcessors { get; } = new List<Func<Message, IntPtr>>();

        public RunService(RunWindowStrategy runWindowStrategy)
        {
            RunWindowStrategy = runWindowStrategy;
        }

        private IEnumerable<Func<Message, IntPtr>> AllMessageProcessors
            => MessageProcessors
                .Select<IMessageProcessor, Func<Message, IntPtr>>(mp => mp.OnMessage)
                .Concat(FuncMessageProcessors);

        public void AddFuncMessageProcessor(Func<Message, IntPtr> funcMessageProcessor)
            => FuncMessageProcessors.Add(funcMessageProcessor);

        public Action<string> OnLogLine { get; set; }

        public void Init()
        {
            switch (RunWindowStrategy)
            {
                case RunWindowStrategy.Win32:
                    ClassName = "Fenester::KeyService-" + Guid.NewGuid().ToString();
                    Handle = Win32Window.CreateWindow(OnMessage, ClassName);
                    break;

                case RunWindowStrategy.WinForms:
                    var form = new Form();
                    Handle = form.Handle;
                    break;

                case RunWindowStrategy.NoWindow:
                default:
                    Handle = IntPtr.Zero;
                    break;
            }

            this.LogLine("RunService.Init : ({0}) => {1}", ClassName, Handle.ToRepr());

            //if (Handle != IntPtr.Zero)
            //{
            //    Win32.ShowWindow(Handle, SW.MINIMIZE);
            //}
        }

        public void Uninit()
        {
            this.LogLine("RunService.Uninit : {0}", Handle.ToRepr());
            if (Handle != IntPtr.Zero)
            {
                Win32.DestroyWindow(Handle);
                if (ClassName != null)
                {
                    Win32.UnregisterClass(ClassName, IntPtr.Zero);
                }
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