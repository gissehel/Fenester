using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Os;

namespace Fenester.Lib.Test.Tools.Win
{
    public static class ConvertTestHelper
    {
        public static string ToRepr(this IRectangle self) => string.Format("[{0}]", self.Canonical);

        public static string ToRepr(this IWindow self) => string.Format("{0} [{1}] [{2}]", self.Canonical, self.Class, self.Title);
    }
}