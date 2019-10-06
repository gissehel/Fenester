using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Win.Service.Helpers;
using System;
using System.Collections.Generic;

namespace Fenester.Lib.Win.Service
{
    public class ScreenOsService : IScreenOsService, ITracable
    {
        public Action<string> OnLogLine { get; set; }

        public IEnumerable<IInternalScreen> GetScreens()
        {
            return Win32Monitor.GetMonitors();
        }

        public void Init()
        {
        }

        public void Uninit()
        {
        }
    }
}