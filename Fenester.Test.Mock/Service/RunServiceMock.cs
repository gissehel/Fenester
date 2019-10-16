using Fenester.Lib.Core.Service;
using System;

namespace Fenester.Test.Mock.Service
{
    public class RunServiceMock : IRunService
    {
        public void Init()
        {
        }

        public void Uninit()
        {
        }

        public bool Running { get; set; } = false;

        public void Run()
        {
            Running = true;
        }

        public void RunFor(TimeSpan timeSpan)
        {
            Running = true;
        }

        public void Stop()
        {
            Running = false;
        }
    }
}