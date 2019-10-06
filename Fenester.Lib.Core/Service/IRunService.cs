using System;

namespace Fenester.Lib.Core.Service
{
    public interface IRunService : IInitializable
    {
        void RunFor(TimeSpan timeSpan);

        void Run();

        void Stop();
    }
}