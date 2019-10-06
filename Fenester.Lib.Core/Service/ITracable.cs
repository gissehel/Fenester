using System;

namespace Fenester.Lib.Core.Service
{
    public interface ITracable
    {
        Action<string> OnLogLine { get; set; }
    }
}