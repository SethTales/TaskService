using System;
using System.Threading.Tasks;

namespace TaskService.Implementations
{
    public class FixedCountTaskService : TaskService
    {
        private readonly Action _runnable;

        internal FixedCountTaskService(int parallelTaskLimit, Action runnable) : base(parallelTaskLimit)
        {
            _runnable = runnable;
        }

        protected override Task CreateTask()
        {
            return new Task(_runnable);
        }
    }
}
