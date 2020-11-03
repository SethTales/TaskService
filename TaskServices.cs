using System;
using System.Collections.Generic;
using TaskService.Implementations;
using TaskService.Interfaces;

namespace TaskService
{
    public class TaskServices
    {
        public static ITaskService OfFixedCountTaskService(int parallelCount, Action runnable)
        {
            return new FixedCountTaskService(parallelCount, runnable);
        }

        public static ITaskService OfBatchingTaskService<T>(int parallelCount, int batchThreshold, IEnumerable<T> input, Action<IEnumerable<T>> runnable)
        {
            return new BatchingTaskService<T>(parallelCount, batchThreshold, input, runnable);
        }
    }
}
