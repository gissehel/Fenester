using Fenester.Lib.Business.Service;
using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Utils;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Test.Tools.Win;
using Fenester.Test.Mock.Domain.Os;
using Fenester.Test.Mock.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fenester.Test.Application
{
    [TestClass]
    public class FenesterTest : DebuggableTest
    {
        public ScreenOsServiceMock ScreenOsServiceImpl { get; set; }
        public IScreenOsServiceMock ScreenOsServiceMock => ScreenOsServiceImpl;
        public IScreenOsService ScreenOsService => ScreenOsServiceImpl;

        public WindowOsServiceMock WindowOsServiceImpl { get; set; }
        public IWindowOsServiceSyncMock WindowOsServiceSyncMock => WindowOsServiceImpl;
        public IWindowOsServiceSync WindowOsService => WindowOsServiceImpl;

        public KeyServiceMock KeyServiceImpl { get; set; }
        public IKeyEmitter KeyEmitter => KeyServiceImpl;
        public IKeyServiceMock KeyServiceMock => KeyServiceImpl;
        public IKeyEmitterMock KeyEmitterMock => KeyServiceImpl;
        public IKeyService KeyService => KeyServiceImpl;

        public DesktopRepository DesktopRepositoryImpl { get; set; }
        public IDesktopRepository DesktopRepository => DesktopRepositoryImpl;

        public ScreenRepository ScreenRepositoryImpl { get; set; }
        public IScreenRepository ScreenRepository => ScreenRepositoryImpl;

        public WindowRepository WindowRepositoryImpl { get; set; }
        public IWindowRepository WindowRepository => WindowRepositoryImpl;

        public FenesterService FenesterServiceImpl { get; set; }
        public IFenesterService FenesterService => FenesterServiceImpl;

        public RunServiceMock RunServiceImpl { get; set; }
        public IRunService RunService => RunServiceImpl;

        protected override void CreateComponents()
        {
            ScreenRepositoryImpl = new ScreenRepository();
            DesktopRepositoryImpl = new DesktopRepository();
            WindowRepositoryImpl = new WindowRepository(new WindowIdMock.WindowIdMockEqualityComparer());
            ScreenOsServiceImpl = new ScreenOsServiceMock();
            WindowOsServiceImpl = new WindowOsServiceMock();
            KeyServiceImpl = new KeyServiceMock();
            FenesterServiceImpl = new FenesterService(ScreenRepository, DesktopRepository, WindowRepository, ScreenOsService, WindowOsService, KeyService, RunService);

            AddComponent(ScreenRepositoryImpl);
            AddComponent(DesktopRepositoryImpl);
            AddComponent(WindowRepositoryImpl);
            AddComponent(ScreenOsServiceImpl);
            AddComponent(WindowOsServiceImpl);
            AddComponent(KeyServiceImpl);
            AddComponent(FenesterServiceImpl);
        }

        [TestMethod]
        public void SimpleTest()
        {
            WindowOsServiceSyncMock.AddWindow(new WindowMock("Terminal 1", "Terminal", "Terminal1", 500, 200, 100, 100));
            WindowOsServiceSyncMock.AddWindow(new WindowMock("Terminal 2", "Terminal", "Terminal2", 800, 150, 80, 120));
            WindowOsServiceSyncMock.AddWindow(new WindowMock("Application 1", "App", "XApp", 600, 400, 150, 110));
            ScreenOsServiceMock.Add(new InternalScreenMock("Screen0", "Screen 0", 1920, 1080, 0, 0));
            ScreenOsServiceMock.Add(new InternalScreenMock("Screen1", "Screen 1", 1920, 1080, 1920, 0));

            FenesterService.Start();

            var window1 = WindowRepository.GetWindow(new WindowIdMock("Terminal1")).WaitAndResult();
            var window2 = WindowRepository.GetWindow(new WindowIdMock("Terminal2")).WaitAndResult();
            var window3 = WindowRepository.GetWindow(new WindowIdMock("XApp")).WaitAndResult();

            Assert.IsNotNull(window1);
            Assert.IsNotNull(window2);
            Assert.IsNotNull(window3);

            Assert.AreEqual(0, window1.RectangleCurrent.Left());
            Assert.AreEqual(640, window2.RectangleCurrent.Left());
            Assert.AreEqual(1280, window3.RectangleCurrent.Left());

            Assert.AreEqual(640, window1.RectangleCurrent.Width());
            Assert.AreEqual(640, window2.RectangleCurrent.Width());
            Assert.AreEqual(640, window3.RectangleCurrent.Width());

            Assert.AreEqual(0, window1.RectangleCurrent.Top());
            Assert.AreEqual(0, window2.RectangleCurrent.Top());
            Assert.AreEqual(0, window3.RectangleCurrent.Top());

            Assert.AreEqual(1080, window1.RectangleCurrent.Height());
            Assert.AreEqual(1080, window2.RectangleCurrent.Height());
            Assert.AreEqual(1080, window3.RectangleCurrent.Height());

            // KeyEmitter.Emit(KeyServiceMock.Keys);
        }
    }
}