using Fenester.Lib.Core.Domain.Utils;
using Fenester.Lib.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fenester.Lib.Core.Service
{
    public abstract class ComponentManager : IComponentManager
    {
        private bool HasUninit {get;set;} = false;

        public Action<string> OnLogLine { get; set; }

        protected virtual ComponentExpressions GetComponentExpressions => new ComponentExpressions();

        protected IList<IComponent> Components { get; } = new List<IComponent>();

        protected void AddComponent(IComponent component)
        {
            Components.Add(component);
        }    

        protected virtual void CreateComponents()
        {
        }

        protected virtual void CreateTraces()
        {
        }

        protected virtual void DisposeTraces() { }

        protected virtual void InitTracesPre()
        {
        }

        protected virtual void InitTracesPost()
        {
        }

        protected virtual void UninitTracesPre()
        {
        }

        protected virtual void UninitTracesPost()
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

        public void InitTraces()
        {
            foreach (var component in Components)
            {
                component.OnLogLine = this.OnLogLine;
            }
        }

        public void UninitTraces()
        {
            foreach (var component in Components)
            {
                if (component != null)
                {
                    component.OnLogLine = null;
                }
            }
        }

        public void InitServices()
        {
            foreach (var component in Components)
            {
                component.Init();
            }
        }

        public void UninitServices()
        {
            if (! HasUninit)
            {
                HasUninit = true;

                foreach (var component in Components)
                {
                    if (component != null)
                    {
                        component.Uninit();
                    }
                }
            }
        }

        public void Init()
        {
            CreateComponents();

            CreateTraces();

            InitTracesPre();
            InitTraces();
            InitTracesPost();

            this.LogLine("Start Init");

            InitServicesPre();
            InitServices();
            InitServicesPost();

            this.LogLine("Start Working");
        }

        public void Uninit()
        {
            this.LogLine("Stop Working");

            UninitServicesPre();
            UninitServices();
            UninitServicesPost();

            this.LogLine("Stop Uninit");

            UninitTracesPre();
            UninitTraces();
            UninitTracesPost();

            OnLogLine = null;

            DisposeTraces();
        }
    }
}
