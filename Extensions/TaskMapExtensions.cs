using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskService.Extensions
{
    internal static class TaskMapExtensions
    {
        internal static int ActiveTasks(this ConcurrentDictionary<int, Task> taskMap)
        {
            return taskMap.Values.Count(t => !t.IsCompleted);
        }

        internal static bool ContainsActiveTasks(this ConcurrentDictionary<int, Task> taskMap)
        {
            return taskMap.Values.Any(t => !t.IsCompleted);
        }
    }
}
