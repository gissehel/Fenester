using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fenester.Exe.InteractiveTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var application = new Application();
            try
            {
                application.Init();
            }
            finally
            {
                application.Uninit();
            }
        }
    }
}
