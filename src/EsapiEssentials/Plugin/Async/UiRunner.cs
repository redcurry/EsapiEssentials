using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Windows.Threading;

namespace EsapiEssentials.Plugin
{
    public class UiRunner : IDisposable
    {
        private readonly DispatcherFrame _frame = new DispatcherFrame();
        private readonly StaTaskScheduler _scheduler = new StaTaskScheduler(1);

        /// <summary>
        /// Runs the given Action on a new UI thread.
        /// </summary>
        /// <param name="a">The Action to run, passing a DispatcherWorker,
        /// which allows the Action to run code on the calling thread.</param>
        public void Run(Action a)
        {
            Task.Factory.StartNew(() =>
            {
                a();
                _frame.Continue = false;
            }, CancellationToken.None, TaskCreationOptions.None, _scheduler);

            Dispatcher.PushFrame(_frame);
        }

        /// <summary>
        /// Runs the given Func on a new UI thread.
        /// </summary>
        /// <param name="f">The Func to run, passing a DispatcherWorker,
        /// which allows the Func to run code on the calling thread.</param>
        public void Run(Func<Task> f)
        {
            Task.Factory.StartNew(async () =>
            {
                await f();
                _frame.Continue = false;
            }, CancellationToken.None, TaskCreationOptions.None, _scheduler);

            Dispatcher.PushFrame(_frame);
        }

        public void Dispose()
        {
            _scheduler?.Dispose();
        }
    }
}
