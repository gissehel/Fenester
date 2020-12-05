using System;

namespace Fenester.Lib.Core.Service
{
    public interface IRunService : IComponent
    {
        void RunFor(TimeSpan timeSpan);

        void Run();

        void Stop();
    }
}