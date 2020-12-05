using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Service;
using Fenester.Lib.Graphical.Domain.Graphical;
using Fenester.Lib.Graphical.Service;
using Fenester.Lib.Test.Tools.Win;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fenester.Lib.Graphical.Test
{
    [TestClass]
    public class RectangleServiceTest : DebuggableTest<RectangleService, IRectangleService>
    {
        protected override void CreateComponents()
        {
            ServiceImpl = new RectangleService();
            AddComponent(Service);
        }

        public void IntersectTest(IRectangle rectangle1, IRectangle rectangle2, IRectangle rectangleExpected)
        {
            TraceFile.SetName("IntersectTest");
            var result = Service.Intersect(rectangle1, rectangle2);
            if (rectangleExpected == null)
            {
                Assert.IsNull(result);
            }
            else
            {
                Assert.AreEqual(rectangleExpected.Left(), result.Left(), 0, "Left");
                Assert.AreEqual(rectangleExpected.Top(), result.Top(), 0, "Top");
                Assert.AreEqual(rectangleExpected.Width(), result.Width(), 0, "Width");
                Assert.AreEqual(rectangleExpected.Height(), result.Height(), 0, "Height");
            }
        }

        [TestMethod]
        public void IntersectTests()
        {
            IntersectTest(new Rectangle(12, 13, 7, 10), new Rectangle(10, 10, 6, 4), new Rectangle(9, 4, 7, 10));
            IntersectTest(new Rectangle(12, 13, 7, 10), new Rectangle(10, 5, 6, 4), null);

            IntersectTest(new Rectangle(12, 13, 7, 10), new Rectangle(4, 10, 1, 4), null);
            IntersectTest(new Rectangle(12, 13, 7, 10), new Rectangle(4, 5, 4, 4), null);

            IntersectTest(new Rectangle(12, 13, 7, 10), new Rectangle(30, 10, 6, 4), new Rectangle(12, 4, 7, 10));
            IntersectTest(new Rectangle(12, 13, 7, 10), new Rectangle(30, 5, 6, 4), null);

            IntersectTest(new Rectangle(12, 13, 7, 10), new Rectangle(30, 10, 9, 4), new Rectangle(10, 4, 9, 10));
            IntersectTest(new Rectangle(12, 13, 7, 10), new Rectangle(30, 5, 9, 4), null);
        }
    }
}