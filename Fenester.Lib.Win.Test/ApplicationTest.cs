using Fenester.Lib.Core.Service;
using Fenester.Lib.Test.Tools.Win;
using Fenester.Lib.Win.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Fenester.Lib.Win.Test
{
    [TestClass]
    public class ApplicationTest : DebuggableTest
    {
        public RunService RunServiceImpl { get; set; }

        public IRunService RunService => RunServiceImpl;

        public KeyService KeyServiceImpl { get; set; }

        public IKeyService KeyService => KeyServiceImpl;

        public WindowOsService WindowOsServiceImpl { get; set; }

        public IWindowOsService WindowOsService => WindowOsServiceImpl;

        #region DebuggableTest

        protected override IEnumerable<ITracable> Tracables => base.Tracables;

        protected override void CreateServices()
        {
            base.CreateServices();
        }

        protected override void InitServices()
        {
            base.InitServices();
        }

        protected override void InitTraces()
        {
            base.InitTraces();
        }

        protected override void UninitServices()
        {
            base.UninitServices();
        }

        protected override void UninitTraces()
        {
            base.UninitTraces();
        }

        #endregion DebuggableTest
    }
}