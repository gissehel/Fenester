using Fenester.Lib.Win.Test;

namespace Fenester.Exe.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var test = new KeyServiceRawInputTest();
            test.TestInitialize();
            try
            {
                test.RegisterShortcutTestNoTimeout();
            }
            finally
            {
                test.TestCleanup();
            }
        }
    }
}