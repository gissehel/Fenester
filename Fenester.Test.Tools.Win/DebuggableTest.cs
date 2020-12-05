using Fenester.Lib.Core.Domain.Utils;
using Fenester.Lib.Core.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Fenester.Lib.Test.Tools.Win
{
    [TestClass]
    public abstract class DebuggableTest : ComponentManager
    {
        public TraceFile TraceFile { get; set; }

        protected override void CreateTraces()
        {
            base.CreateTraces();
            TraceFile = new TraceFile();
            OnLogLine = (line) => TraceFile.OutLine(line);
            Tracable.DefaultOnLogLine = OnLogLine;
        }

        protected override void DisposeTraces()
        {
            base.DisposeTraces();
            TraceFile.Close();
            TraceFile = null;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Init();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Uninit();
        }
    }

    public abstract class DebuggableTest<T, IT> : DebuggableTest, IComponent where T : IT, IComponent
    {
        public T ServiceImpl { get; set; }

        public IT Service => ServiceImpl;

    }
}