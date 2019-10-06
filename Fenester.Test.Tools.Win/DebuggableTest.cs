using Fenester.Lib.Core.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

        protected virtual void InitServicesPre()
        {
        }

        protected virtual void InitServicesPost()
        {
        }

        protected virtual void UninitServicesPre()
        {
        }

        protected virtual void UninitServicesPost()
        {
        }

        protected virtual void UninitTraces()
        {
        }

        protected virtual IEnumerable<ITracable> Tracables => new List<ITracable> { };

        protected virtual IEnumerable<Expression<Func<IInitializable>>> GetInitializaleExpressions => new List<Expression<Func<IInitializable>>>();

        private PropertyInfo GetProperty(string propertyName)
        {
            var type = GetType();
            var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return property;
        }

        private Func<IInitializable> GetInitializableGetter(string propertyName)
        {
            var property = GetProperty(propertyName);
            var method = property.GetMethod;
            var getter = (Func<IInitializable>)Delegate.CreateDelegate(typeof(Func<IInitializable>), this, method);
            return getter;
        }

        private Action<IInitializable> GetInitializableSetter(string propertyName)
        {
            var property = GetProperty(propertyName);
            var method = property.SetMethod;
            return new Action<IInitializable>((value) => method.Invoke(this, new object[] { value }));
        }

        protected void InitServices()
        {
            var initializables = GetInitializaleExpressions
                .Select(x => x.GetPropertyName())
                .Select(x => GetInitializableGetter(x)())
                .Where(x => x != null)
            ;
            foreach (var initializable in initializables)
            {
                initializable.Init();
            }
        }

        protected void UninitServices()
        {
            var initializablePropertyNames = GetInitializaleExpressions
                .Select(x => x.GetPropertyName())
                .Reverse();

            foreach (var initializablePropertyName in initializablePropertyNames)
            {
                var initializableGetter = GetInitializableGetter(initializablePropertyName);
                var initializableSetter = GetInitializableSetter(initializablePropertyName);
                var initializable = initializableGetter();

                if (initializable != null)
                {
                    initializable.Uninit();
                }
                initializableSetter(null);
            }
        }

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

            InitServicesPre();
            InitServices();
            InitServicesPost();

            this.LogLine("Start Test");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.LogLine("Stop Test");

            UninitServicesPre();
            UninitServices();
            UninitServicesPost();

            this.LogLine("Stop Uninit");

            UninitTraces();

            OnLogLine = null;

            TraceFile.Close();
            TraceFile = null;
        }
    }

    public abstract class DebuggableTest<T, IT> : DebuggableTest, ITracable where T : IT, ITracable, IInitializable
    {
        public T ServiceImpl { get; set; }

        public IT Service => ServiceImpl;

        protected override IEnumerable<ITracable> Tracables
            => new List<ITracable> { ServiceImpl };

        protected override IEnumerable<Expression<Func<IInitializable>>> GetInitializaleExpressions
            => new List<Expression<Func<IInitializable>>>() { () => ServiceImpl, };
    }
}