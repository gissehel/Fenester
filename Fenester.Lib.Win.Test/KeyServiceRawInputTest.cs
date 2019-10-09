using Fenester.Lib.Win.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fenester.Lib.Win.Test
{
    [TestClass]
    public class KeyServiceRawInputTest : KeyServiceTestBase<KeyServiceRawInput>
    {
        protected override KeyServiceRawInput CreateMainService() => new KeyServiceRawInput(RunServiceImpl);
    }
}