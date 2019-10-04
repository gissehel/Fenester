using Fenester.Lib.Win.Service.Helpers;

namespace Fenester.Lib.Win.Test
{
    public static class Flags
    {
        public static FlagAnalyser<WS> FlagAnalyserWS { get; set; } = new FlagAnalyser<WS>()
            .Add("WS_OVERLAPPED", WS.OVERLAPPED)
            .Add("WS_POPUP", WS.POPUP)
            .Add("WS_CHILD", WS.CHILD)
            .Add("WS_MINIMIZE", WS.MINIMIZE)
            .Add("WS_VISIBLE", WS.VISIBLE)
            .Add("WS_DISABLED", WS.DISABLED)
            .Add("WS_CLIPSIBLINGS", WS.CLIPSIBLINGS)
            .Add("WS_CLIPCHILDREN", WS.CLIPCHILDREN)
            .Add("WS_MAXIMIZE", WS.MAXIMIZE)
            .Add("WS_BORDER", WS.BORDER)
            .Add("WS_DLGFRAME", WS.DLGFRAME)
            .Add("WS_VSCROLL", WS.VSCROLL)
            .Add("WS_HSCROLL", WS.HSCROLL)
            .Add("WS_SYSMENU", WS.SYSMENU)
            .Add("WS_THICKFRAME", WS.THICKFRAME)
            .Add("WS_GROUP", WS.GROUP)
            .Add("WS_TABSTOP", WS.TABSTOP)
            .Add("WS_MINIMIZEBOX", WS.MINIMIZEBOX)
            .Add("WS_MAXIMIZEBOX", WS.MAXIMIZEBOX)
        ;

        public static FlagAnalyser<WS_EX> FlagAnalyserWSEX { get; set; } = new FlagAnalyser<WS_EX>()
            .Add("WS_EX_DLGMODALFRAME", WS_EX.DLGMODALFRAME)
            .Add("WS_EX_NOPARENTNOTIFY", WS_EX.NOPARENTNOTIFY)
            .Add("WS_EX_TOPMOST", WS_EX.TOPMOST)
            .Add("WS_EX_ACCEPTFILES", WS_EX.ACCEPTFILES)
            .Add("WS_EX_TRANSPARENT", WS_EX.TRANSPARENT)
            .Add("WS_EX_MDICHILD", WS_EX.MDICHILD)
            .Add("WS_EX_TOOLWINDOW", WS_EX.TOOLWINDOW)
            .Add("WS_EX_WINDOWEDGE", WS_EX.WINDOWEDGE)
            .Add("WS_EX_CLIENTEDGE", WS_EX.CLIENTEDGE)
            .Add("WS_EX_CONTEXTHELP", WS_EX.CONTEXTHELP)
            .Add("WS_EX_RIGHT", WS_EX.RIGHT)
            .Add("WS_EX_RTLREADING", WS_EX.RTLREADING)
            .Add("WS_EX_LEFTSCROLLBAR", WS_EX.LEFTSCROLLBAR)
            .Add("WS_EX_CONTROLPARENT", WS_EX.CONTROLPARENT)
            .Add("WS_EX_STATICEDGE", WS_EX.STATICEDGE)
            .Add("WS_EX_APPWINDOW", WS_EX.APPWINDOW)
            .Add("WS_EX_LAYERED", WS_EX.LAYERED)
            .Add("WS_EX_NOINHERITLAYOUT", WS_EX.NOINHERITLAYOUT)
            .Add("WS_EX_NOREDIRECTIONBITMAP", WS_EX.NOREDIRECTIONBITMAP)
            .Add("WS_EX_LAYOUTRTL", WS_EX.LAYOUTRTL)
            .Add("WS_EX_COMPOSITED", WS_EX.COMPOSITED)
            .Add("WS_EX_NOACTIVATE", WS_EX.NOACTIVATE)
        ;
    }
}