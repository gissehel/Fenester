using Fenester.Lib.Core.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fenester.Lib.Test.Tools.Win
{
    [TestClass]
    public abstract class DebuggableTest : ITracable
    {
        public Action<string> OnLogLine { get; set; }

        public TraceFile TraceFile { get; set; }

        public void TraceClasses(params ITracable[] tracables)
        {
            foreach (var tracable in tracables)
            {
                tracable.OnLogLine = OnLogLine;
            }
        }

        protected virtual void CreateServices()
        {
        }

        protected virtual void InitTraces()
        {
        }

        protected virtual void InitServices()
        {
        }

        protected virtual void UninitServices()
        {
        }

        protected virtual void UninitTraces()
        {
        }

        protected virtual IEnumerable<ITracable> Tracables => new List<ITracable> { };

        [TestInitialize]
        public void TestInitialize()
        {
            CreateServices();

            TraceFile = new TraceFile();
            OnLogLine = (line) => TraceFile.OutLine(line);
            Tracable.DefaultOnLogLine = OnLogLine;

            TraceClasses(Tracables.ToArray());

            InitTraces();

            this.LogLine("Start Init");

            InitServices();

            this.LogLine("Start Test");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.LogLine("Stop Test");

            UninitServices();

            this.LogLine("Stop Uninit");

            UninitTraces();

            OnLogLine = null;

            TraceFile.Close();
            TraceFile = null;
        }
    }

    public abstract class DebuggableTest<T, IT> : DebuggableTest, ITracable where T : IT, ITracable
    {
        public T ServiceImpl { get; set; }

        public IT Service => ServiceImpl;

        protected override IEnumerable<ITracable> Tracables => new List<ITracable> { ServiceImpl };
    }
}