using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fenester.Lib.Test.Tools.Win
{
    public class TraceFile
    {
        private TextWriter TextWriter { get; set; }

        private bool Opened { get; set; }

        private List<string> Buffer { get; set; } = new List<string>();

        public string Name { get; private set; }

        private string IdStamp { get; set; }

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

        private void EnsureOpen()
        {
            if (!Opened)
            {
                string filename = null;
                if (IdStamp == null)
                {
                    const string datePattern = "yyyyMMdd-HHmmss";
                    IdStamp = DateTime.Now.ToString(datePattern);
                }
                filename = string.Format("Out-{0}{1}{2}.log", IdStamp, Name != null ? "-" : "", Name);
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
            EnsureOpen();
            if (Buffer.Count > 0)
            {
                Buffer.ForEach((line) => Writer.WriteLine(line));
                Buffer.Clear();
                Writer.Flush();
            }
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
            AddLine(string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), line));
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

        ~TraceFile()
        {
            Close();
        }
    }
}