using Fenester.Lib.Core.Domain.Os;

namespace Fenester.Lib.Core.Domain.Fenester
{
    public interface IDesktop
    {
        string Name { get; set; }

        string Id { get; set; }

        IInternalScreen Screen { get; set; }
    }
}