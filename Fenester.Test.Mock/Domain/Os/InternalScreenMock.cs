using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Graphical.Domain.Graphical;

namespace Fenester.Test.Mock.Domain.Os
{
    public class InternalScreenMock : IInternalScreen
    {
        public InternalScreenMock(string id, string name, int width, int height, int left, int top)
        {
            Id = id;
            Name = name;
            Rectangle = new Rectangle(width, height, left, top);
        }

        public string Id { get; set; }

        public IRectangle Rectangle { get; set; }

        public string Name { get; set; }

        public int Index { get; set; }

        public void UpdateFrom(IScreen screen)
        {
            Index = screen.Index;
            Name = screen.Name;
            Rectangle = screen.Rectangle.Clone();
        }
    }
}