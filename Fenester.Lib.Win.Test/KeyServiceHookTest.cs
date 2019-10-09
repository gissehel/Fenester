using Fenester.Lib.Win.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fenester.Lib.Win.Test
{
    [TestClass]
    public class KeyServiceHookTest : KeyServiceTestBase<KeyServiceHook>
    {
        protected override KeyServiceHook CreateMainService() => new KeyServiceHook();
    }
}