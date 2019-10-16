using System.Threading.Tasks;

namespace Fenester.Test.Application
{
    public static class TaskHelper
    {
        public static T WaitAndResult<T>(this Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
    }
}