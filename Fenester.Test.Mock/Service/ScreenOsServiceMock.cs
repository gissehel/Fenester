using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Service;
using Fenester.Test.Mock.Domain.Os;
using System;
using System.Collections.Generic;

namespace Fenester.Test.Mock.Service
{
    public class ScreenOsServiceMock : IScreenOsService, IScreenOsServiceMock
    {
        public Action<string> OnLogLine { get; set; }
        public Action OnInit { get; set; }
        public Action OnUninit { get; set; }

        public Func<IEnumerable<IInternalScreen>> OnGetScreens { get; set; }

        public List<InternalScreenMock> Screens { get; } = new List<InternalScreenMock>();

        public void Add(InternalScreenMock internalScreen)
        {
            internalScreen.Index = Screens.Count;
            Screens.Add(internalScreen);
        }

        public IEnumerable<IInternalScreen> GetScreens()
        {
            if (OnGetScreens != null)
            {
                return OnGetScreens();
            }
            else
            {
                return Screens;
            }
        }

        public void Init()
        {
            OnInit?.Invoke();
        }

        public void Uninit()
        {
            OnUninit?.Invoke();
        }
    }
}