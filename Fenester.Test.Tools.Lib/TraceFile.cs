using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fenester.Lib.Test.Tools.Win
{
    public class TraceFile : IDisposable
    {
        private TextWriter TextWriter { get; set; }

        private bool Opened { get; set; }

        private List<string> Buffer { get; set; } = new List<string>();

        public string Name { get; private set; }

        private string IdStamp { get; set; }

        private bool IncludeTimestamp { get; set; }

        private string _extension = null;

        private string Extension
        {
            get
            {
                if (_extension == null)
                {
                    return "log";
                }
                else
                {
                    return _extension;
                }
            }
            set
            {
                _extension = value;
            }
        }

        private TextWriter Writer
        {
            get
            {
                EnsureOpen();
                return TextWriter;
            }
        }

        public static TraceFile Get(string name)
        {
            return new TraceFile().SetName(name);
        }

        public static TraceFile Get(string name, string extension)
        {
            return new TraceFile().SetExtension(extension).SetName(name);
        }

        private void EnsureOpen()
        {
            if (!Opened)
            {
                if (IdStamp == null)
                {
                    const string datePattern = "yyyyMMdd-HHmmss";
                    IdStamp = DateTime.Now.ToString(datePattern);
                }
                string filename = string.Format("Out-{0}{1}{2}.{3}", IdStamp, Name != null ? "-" : "", Name, Extension);
                TextWriter = new StreamWriter(filename, true, Encoding.UTF8);
                Opened = true;
            }
        }

        public TraceFile SetName(string name)
        {
            if (Opened)
            {
                Close();
            }
            Name = name;
            if (Buffer.Count > 0)
            {
                EnsureOpen();
                Buffer.ForEach((line) => Writer.WriteLine(line));
                Buffer.Clear();
                Writer.Flush();
            }
            return this;
        }

        public TraceFile SetExtension(string extension)
        {
            if (Opened)
            {
                Close();
            }
            Extension = extension;
            return this;
        }

        public TraceFile SetIncludeTimestamp(bool includeTimestamp)
        {
            IncludeTimestamp = includeTimestamp;
            return this;
        }

        public TraceFile()
        {
            TextWriter = null;
            Opened = false;
        }

        private void AddLine(string line)
        {
            if (Name == null)
            {
                Buffer.Add(line);
            }
            else
            {
                Writer.WriteLine(line);
                Writer.Flush();
            }
        }

        public TraceFile OutLine(string line)
        {
            if (IncludeTimestamp)
            {
                AddLine(string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), line));
            }
            else
            {
                AddLine(line);
            }
            return this;
        }

        public TraceFile OutLine(string format, params object[] args)
        {
            var line = string.Format(format, args);
            return OutLine(line);
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
            if (Buffer != null && Buffer.Count > 0 && !Opened)
            {
                if (Name == null)

                {
                    Name = "__Close__";
                }
                EnsureOpen();
                Buffer.ForEach((line) => TextWriter.WriteLine(line));
                Buffer.Clear();
            }
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

        public void Dispose()
        {
            Close();
        }

        ~TraceFile()
        {
            Close();
        }
    }
}