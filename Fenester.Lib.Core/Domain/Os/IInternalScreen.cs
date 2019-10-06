namespace Fenester.Lib.Core.Domain.Os
{
    public interface IInternalScreen : IScreen, IModifiableScreen
    {
        string Id { get; }
    }
}