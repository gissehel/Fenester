using Fenester.Lib.Core.Service;
using Fenester.Lib.Win.Domain.Key;
using Fenester.Lib.Win.Service.Helpers;
using System;
using System.Linq;

namespace Fenester.Lib.Win.Service
{
    public class KeyServiceHotKey : KeyServiceBase<Keys>, IMessageProcessor
    {
        private IRunServiceWin RunService { get; set; }

        private IntPtr Handle => RunService.Handle;

        public KeyServiceHotKey(IRunServiceWin runService) => RunService = runService;

        public override void Init()
        {
            this.LogLine("KeyService.Init()");
            RunService.RegisterMessageProcessor(this);
        }

        public override void Uninit()
        {
            this.LogLine("KeyService.Uninit()");
            RunService.UnregisterMessageProcessor(this);
            foreach (var registeredShortcut in RegisteredShortcuts.Values.ToList())
            {
                UnregisterShortcut(registeredShortcut);
            }
            RegisteredShortcuts.Clear();
        }

        public IntPtr OnMessage(Message message)
        {
            this.LogLine("Message on {0} : {1}", message.handle.ToRepr(), message.message.ToRepr());
            switch (message.message)
            {
                case WM.HOTKEY:
                    int id = message.wParam.ToInt32();
                    if (RegisteredShortcuts.ContainsKey(id))
                    {
                        ExecuteRegisteredShortcut(RegisteredShortcuts[id]);
                    }
                    break;

                default:
                    break;
            }

            return IntPtr.Zero;
        }

        protected override bool RegisterHotKey(Shortcut<Keys> shortcut, int id)
        {
            return Win32.RegisterHotKey
            (
                Handle,
                id,
                shortcut.KeyModifier.ToKeyModifiers(),
                shortcut.Key.Value
            );
        }

        protected override bool UnregisterHotKey(Shortcut<Keys> shortcut, int id)
        {
            Win32.UnregisterHotKey(Handle, id);
            return true;
        }
    }
}