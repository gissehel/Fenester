using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fenester.Lib.Business.Service
{
    public class FenesterService : IFenesterService, IInitializable
    {
        private IScreenRepository ScreenRepository { get; }
        private IDesktopRepository DesktopRepository { get; }
        private IWindowRepository WindowRepository { get; }
        private IScreenOsService ScreenOsService { get; }
        private IWindowOsServiceSync WindowOsService { get; }
        private IKeyService KeyService { get; }
        private IRunService RunService { get; }

        public FenesterService
            (
                IScreenRepository screenRepository,
                IDesktopRepository desktopRepository,
                IWindowRepository windowRepository,
                IScreenOsService screenOsService,
                IWindowOsServiceSync windowOsService,
                IKeyService keyService,
                IRunService runService
            )
        {
            ScreenRepository = screenRepository;
            DesktopRepository = desktopRepository;
            WindowRepository = windowRepository;
            ScreenOsService = screenOsService;
            WindowOsService = windowOsService;
            KeyService = keyService;
        }

        public void Init()
        {
        }

        public void Uninit()
        {
        }

        public List<IInternalScreen> Screens { get; set; }
        public Action<string> OnLogLine { get; set; }

        public void Start()
        {
            StartAsync().Wait();
        }

        public async Task StartAsync()
        {
            var screens = ScreenOsService.GetScreens().OrderBy(s => s.Rectangle.Position.Left).ToList();
            var desktops = await DesktopRepository.GetDesktops();
            var desktop = desktops.First();
            foreach (var screen in screens)
            {
                await ScreenRepository.AddOrUpdateScreen(screen);
                desktop.Screen = screen;

                desktop = await DesktopRepository.GetNext(desktop);
            }
            var windows = WindowOsService.GetWindowsSync();
            foreach (var window in windows)
            {
                await WindowRepository.AddOrUpdateWindow(window);
            }
            ForceLayout();
        }

        private void ForceLayout()
        {
        }
    }
}