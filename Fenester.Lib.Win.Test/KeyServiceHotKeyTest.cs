using Fenester.Lib.Win.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fenester.Lib.Win.Test
{
    [TestClass]
    public class KeyServiceHotKeyTest : KeyServiceTestBase<KeyServiceHotKey>
    {
        protected override KeyServiceHotKey CreateMainService() => new KeyServiceHotKey(RunServiceImpl);
    }
}