using System;
using System.Collections.Generic;
using System.Linq;

namespace Fenester.Lib.Win.Test
{
    public class FlagAnalyser<T> where T : struct
    {
        private class Flag
        {
            public Flag(string name, long value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; }
            public long Value { get; }
        }

        private List<Flag> Flags { get; } = new List<Flag>();

        public FlagAnalyser<T> Add(string name, T value)
        {
            Flags.Add(new Flag(name, Convert.ToInt64(value)));
            return this;
        }

        public IEnumerable<string> GetNames(T value)
        {
            var intValue = Convert.ToInt64(value);
            var mappedFlags = Flags.Where(flag => (flag.Value & intValue) != 0);
            var namedValue = mappedFlags.Select(flag => flag.Value).Aggregate((result, next) => result | next);
            var mappedNames = mappedFlags.Select(flag => flag.Name);
            var unnamedValue = intValue & ~namedValue;
            if (unnamedValue != 0)
            {
                return mappedNames.Concat(new List<string>() { unnamedValue.ToString() });
            };
            return mappedNames;
        }
    }
}