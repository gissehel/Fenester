using Fenester.Lib.Core.Domain.Fenester;
using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Enums;

namespace Fenester.Lib.Business.Domain.Fenester
{
    public class FenesterWindow : IFenesterWindow
    {
        public FenesterWindow(IWindow window)
        {
            Window = window;
        }

        public IWindowId Id => Window.Id;

        public IWindow Window { get; set; }

        public Visibility FenesterVisibility { get; set; }

        public IRectangle TheoricalRectangle { get; set; }

        public bool Floating { get; set; }

        public IDesktop Desktop { get; set; }
    }
}