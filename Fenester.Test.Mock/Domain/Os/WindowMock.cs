using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Enums;
using Fenester.Lib.Graphical.Domain.Graphical;

namespace Fenester.Test.Mock.Domain.Os
{
    public class WindowMock : IWindow, IInternalWindow
    {
        public WindowMock()
        {
        }

        public WindowMock(string title, string @class, string id, int width, int height, int left, int top)
        {
            Title = title;
            Class = @class;
            Id = new WindowIdMock(id);
            Rectangle = new Rectangle(width, height, left, top);
            RectangleCurrent = new Rectangle(width, height, left, top);
            OsVisibility = Visibility.Normal;
            Category = WindowCategory.Normal;
        }

        public WindowIdMock Id { get; set; }

        IWindowId IWindow.Id => Id;

        public string Title { get; set; }

        public string Class { get; set; }

        public IRectangle Rectangle { get; set; }

        public IRectangle RectangleCurrent { get; set; }

        public Visibility OsVisibility { get; set; }

        public WindowCategory Category { get; set; }

        private string RectangleCanonical => Rectangle == null ? "" : Rectangle.Canonical;

        private string RectangleCurrentCanonical => RectangleCurrent == null ? "" : RectangleCurrent.Canonical;

        public string Canonical => string.Format
            (
                "[{0}:{1,20}]",
                Id.Canonical,
                OsVisibility == Visibility.Minimized ? "*" : RectangleCurrentCanonical
            );

        public IInternalWindow Clone()
        {
            return new WindowMock
            {
                Id = new WindowIdMock(Id.RawId),
                Title = Title,
                Class = Class,
                Rectangle = Rectangle.Clone(),
                RectangleCurrent = RectangleCurrent.Clone(),
                OsVisibility = OsVisibility,
                Category = Category,
            };
        }

        IWindow IWindow.Clone() => Clone();

        public void UpdateFrom(IWindow window)
        {
            Title = window.Title;
            Class = window.Class;
            Rectangle = window.Rectangle.Clone();
            RectangleCurrent = window.RectangleCurrent.Clone();
            OsVisibility = window.OsVisibility;
            Category = window.Category;
        }
    }
}