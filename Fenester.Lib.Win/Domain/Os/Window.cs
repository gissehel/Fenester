using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Enums;
using Fenester.Lib.Win.Service.Helpers;
using System;

namespace Fenester.Lib.Win.Domain.Os
{
    public class InitialWindowProps
    {
        public IRectangle RectangleCurrent { get; set; }

        public WS WinStyles { get; set; }

        public WS_EX WinExStyles { get; set; }
    }

    public class Window : IInternalWindow
    {
        public WindowId Id { get; }

        public IntPtr Handle => Id.Handle;

        public Window(IWindowId id)
        {
            Id = id as WindowId;
        }

        internal Window(IntPtr handle)
        {
            Id = new WindowId(handle);
        }

        public string Title { get; set; }

        public string Class { get; set; }

        public IRectangle Rectangle { get; set; }

        public IRectangle RectangleCurrent { get; set; }

        IWindowId IWindow.Id => Id;

        public Visibility OsVisibility { get; set; }

        public WindowCategory Category { get; set; }

        public InitialWindowProps InitialWindowProps { get; set; }

        public void UpdateFrom(IWindow window)
        {
            Title = window.Title;
            Class = window.Class;
            Rectangle = window.Rectangle?.Clone();
            RectangleCurrent = window.RectangleCurrent?.Clone();
            OsVisibility = window.OsVisibility;
            Category = window.Category;
        }

        private string RectangleCanonical => Rectangle == null ? "" : Rectangle.Canonical;
        private string RectangleCurrentCanonical => RectangleCurrent == null ? "" : RectangleCurrent.Canonical;
        public string Canonical => string.Format("[{0}:{1,20}]", Id.Canonical, OsVisibility == Visibility.Minimized ? "*" : RectangleCurrentCanonical);

        public IInternalWindow Clone() => new Window(Id)
        {
            Category = Category,
            Rectangle = Rectangle?.Clone(),
            RectangleCurrent = RectangleCurrent?.Clone(),
            OsVisibility = OsVisibility,
            Title = Title,
            Class = Class
        };

        IWindow IWindow.Clone() => Clone();
    }
}