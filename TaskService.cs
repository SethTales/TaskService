using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using TaskService.Extensions;
using TaskService.Interfaces;

namespace TaskService
{
    public abstract class TaskService : ITaskService
    {
        protected readonly ConcurrentDictionary<int, Task> TaskMap;
        protected readonly int ParallelTaskLimit;

        protected bool IsExecuting;
        protected Task ExecutorThread;

        protected TaskService(int parallelTaskLimit)
        {
            ParallelTaskLimit = parallelTaskLimit;
            TaskMap = new ConcurrentDictionary<int, Task>();
        }

        public Task Execute()
        {
            IsExecuting = true;
            ExecutorThread = new Task(ProcessingLoop);
            ExecutorThread.Start();
            return ExecutorThread;
        }

        public Task Shutdown()
        {
            IsExecuting = false;
            var shutdownTask = new Task(ShutdownLoop);
            shutdownTask.Start();
            return shutdownTask;
        }

        protected virtual void ProcessingLoop()
        {
            while (IsExecuting)
            {
                while (TaskMap.ActiveTasks() < ParallelTaskLimit)
                {
                    var task = CreateTask();
                    TaskMap.TryAdd(task.Id, task);
                    task.Start();
                }
            }
        }

        protected virtual void ShutdownLoop()
        {
            while (TaskMap.ContainsActiveTasks())
            {
                Thread.Sleep(100);
            }
        }

        protected abstract Task CreateTask();

        private void TaskMapMonitorLoop()
        {
            var keys = TaskMap.Keys;
            foreach (var key in keys)
            {
                if (TaskMap[key].IsCompleted)
                {
                    TaskMap.TryRemove(key, out _);
                }
            }
        }
    }
}
