using System;

namespace Fenester.Lib.Core.Service
{
    public static class Tracable
    {
        public static Action<string> DefaultOnLogLine { get; set; } = null;

        public static bool Activated => DefaultOnLogLine != null;

        private static void LogLine(this ITracable tracable, string line)
        {
            if (tracable?.OnLogLine == null)
            {
                if (Activated)
                {
                    DefaultOnLogLine(line);
                }
            }
            else
            {
                tracable?.OnLogLine(line);
            }
        }

        public static void LogLine(this ITracable tracable, string format, params object[] args)
        {
            if (Activated && (tracable != null))
            {
                var line = string.Format(format, args);
                tracable.LogLine(line);
            }
        }
    }
}