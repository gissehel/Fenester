using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fenester.Lib.Business.Service
{
    public class ScreenRepository : IScreenRepository
    {
        private List<IInternalScreen> InternalScreens { get; }
        private Dictionary<string, IInternalScreen> InternalScreensById { get; }

        public ScreenRepository()
        {
            InternalScreens = new List<IInternalScreen>();
            InternalScreensById = new Dictionary<string, IInternalScreen>();
        }

        private void UpdateScreen(IInternalScreen internalScreenToUpdate, IInternalScreen internalScreenExternal)
        {
            internalScreenToUpdate.UpdateFrom(internalScreenExternal);
        }

        public Task AddOrUpdateScreen(IInternalScreen internalScreen)
        {
            var predicateScreen = new Predicate<IInternalScreen>((screen) => screen.Id == internalScreen.Id);
            var internalScreenFound = InternalScreens.Find(predicateScreen);
            if (internalScreenFound != null)
            {
                UpdateScreen(internalScreenFound, internalScreen);
            }
            else
            {
                InternalScreens.Add(internalScreen);
                InternalScreensById[internalScreen.Id] = internalScreen;
            }
            return Task.CompletedTask;
        }

        public Task ChangeOrder(IEnumerable<IScreen> screens)
        {
            var screensToOrder = screens
                .Select(screen => screen as IInternalScreen)
                .Where(screen => screen != null)
                .Where(screen => InternalScreensById.ContainsKey(screen.Id))
            ;
            var screensToOrderIds = new HashSet<string>(screensToOrder.Select(screen => screen.Id));
            var remainingScreens = InternalScreens.Where(screen => !screensToOrderIds.Contains(screen.Id));
            int index = 0;
            foreach (var screen in screensToOrder.Concat(remainingScreens))
            {
                (InternalScreensById[screen.Id] as IModifiableScreen).Index = index;
                index++;
            }
            return Task.CompletedTask;
        }

        public Task<IScreen> GetNext(IScreen screen)
        {
            int index = screen.Index;
            index++;
            index %= InternalScreens.Count;
            return Task.FromResult<IScreen>(InternalScreens[index]);
        }

        public Task<IScreen> GetPrev(IScreen screen)
        {
            int index = screen.Index;
            index += InternalScreens.Count;
            index--;
            index %= InternalScreens.Count;
            return Task.FromResult<IScreen>(InternalScreens[index]);
        }

        public Task<IEnumerable<IScreen>> GetScreens() => Task.FromResult<IEnumerable<IScreen>>(InternalScreens);

        public void Init()
        {
        }

        public void Uninit()
        {
        }
    }
}