namespace Fenester.Lib.Core.Domain.Os
{
    public interface IInternalWindow : IWindow, IModifiableWindow
    {
        new IInternalWindow Clone();
    }
}