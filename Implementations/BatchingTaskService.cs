using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskService.Interfaces;

namespace TaskService.Implementations
{
    public class BatchingTaskService<T> : TaskService
    {
        private readonly int _batchThreshold;
        private readonly IEnumerable<T> _input;
        private readonly Action<IEnumerable<T>> _runnable;

        private int _batchCount;

        internal BatchingTaskService(int parallelCount, int batchThreshold, IEnumerable<T> input, Action<IEnumerable<T>> runnable) : base(parallelCount)
        {
            _batchThreshold = batchThreshold;
            _input = input;
            _runnable = runnable;
        }

        protected override Task CreateTask()
        {
            return new Task(() => _runnable(GetNextBatch()));
        }

        private IEnumerable<T> GetNextBatch()
        {
            if (_input.Count() <= _batchThreshold)
            {
                return _input;
            }

            var batch = _input.Skip(_batchCount * _batchThreshold).Take(_batchThreshold);
            _batchCount++;
            return batch;
        }

    }
}
