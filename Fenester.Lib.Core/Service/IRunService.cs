using System;

namespace Fenester.Lib.Core.Service
{
    public interface IRunService
    {
        void RunFor(TimeSpan timeSpan);

        void Run();

        void Stop();
    }
}