using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Os;

namespace Fenester.Lib.Win.Domain.Os
{
    public class Screen : IInternalScreen
    {
        public Screen()
        {
        }

        public IRectangle Rectangle { get; set; }

        public string Name { get; set; }

        public int Index { get; set; }

        public string Id { get; set; }

        public void UpdateFrom(IScreen window)
        {
            Index = window.Index;
            Name = window.Name;
            Rectangle = window.Rectangle.Clone();
        }
    }
}