using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EsapiEssentials.Plugin
{
    internal class AsyncRunner
    {
        private readonly Dispatcher _dispatcher;

        public AsyncRunner()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public Task RunAsync(Action a) =>
            _dispatcher.InvokeAsync(a).Task;

        public Task<T> RunAsync<T>(Func<T> f) =>
            _dispatcher.InvokeAsync(f).Task;
    }
}