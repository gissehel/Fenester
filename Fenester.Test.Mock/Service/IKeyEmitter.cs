using Fenester.Test.Mock.Domain.Key;

namespace Fenester.Test.Mock.Service
{
    public interface IKeyEmitter
    {
        void Emit(KeyMock key);
    }
}