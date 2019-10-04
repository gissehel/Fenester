using System;
using System.Collections.Generic;
using System.IO;

namespace Fenester.Lib.Win.Test
{
    public class TraceFile
    {
        private TextWriter TextWriter { get; set; }
        private bool Opened { get; set; }
        public string Name { get; private set; }

        private TextWriter Writer
        {
            get
            {
                if (Opened)
                {
                    return TextWriter;
                }
                else
                {
                    string filename = null;
                    const string datePattern = "yyyyMMdd-HHmmss";
                    if (Name != null)
                    {
                        filename = string.Format("Out-{0}-{1}.log", DateTime.Now.ToString(datePattern), Name);
                    }
                    else
                    {
                        filename = string.Format("Out-{0}.log", DateTime.Now.ToString(datePattern));
                    }
                    TextWriter = new StreamWriter(filename);
                    Opened = true;
                    return TextWriter;
                }
            }
        }

        public static TraceFile Get(string name)
        {
            return new TraceFile().SetName(name);
        }

        private TraceFile Open()
        {
            var ignore = Writer;
            return this;
        }

        public TraceFile SetName(string name)
        {
            if (Opened)
            {
                Close();
            }
            Name = name;
            return Open();
        }

        public TraceFile()
        {
            TextWriter = null;
            Opened = false;
        }

        public TraceFile Out(string format, params object[] args)
        {
            var line = string.Format(format, args);
            Writer.WriteLine(line);
            return this;
        }

        public TraceFile Each<T>(IEnumerable<T> items, Action<TraceFile, T> action)
        {
            foreach (var item in items)
            {
                action(this, item);
            }
            return this;
        }

        public void Close()
        {
            if (Opened)
            {
                Opened = false;
                try
                {
                    TextWriter.Close();
                }
                catch
                {
                }
                TextWriter = null;
            }
        }

        ~TraceFile()
        {
            Close();
        }
    }
}