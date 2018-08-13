using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;

namespace EsapiPowerTools.Async
{
    public class EsapiAsyncRunner : IDisposable
    {
        private readonly StaTaskScheduler _taskScheduler;

        public EsapiAsyncRunner()
        {
            _taskScheduler = new StaTaskScheduler(1);
        }

        public Task RunAsync(Action a) =>
            Task.Factory.StartNew(a, CancellationToken.None, TaskCreationOptions.None, _taskScheduler);

        public Task<TResult> RunAsync<TResult>(Func<TResult> f) =>
            Task.Factory.StartNew(f, CancellationToken.None, TaskCreationOptions.None, _taskScheduler);

        public void Dispose()
        {
            _taskScheduler?.Dispose();
        }
    }
}
