using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EsapiEssentials.Plugin
{
    /// <summary>
    /// Provides asynchronous methods (awaitable) to run the given methods on the
    /// Dispatcher for the thread where an instance of this class was created.
    /// </summary>
    public class DispatcherRunner
    {
        private readonly Dispatcher _dispatcher =
            Dispatcher.CurrentDispatcher;

        /// <summary>
        /// Runs the given Func on the Dispatcher.
        /// </summary>
        /// <typeparam name="T">The type of the returned object.</typeparam>
        /// <param name="f">The Func to run.</param>
        /// <returns>A Task to perform the given Func.</returns>
        public Task<T> RunAsync<T>(Func<T> f) =>
            _dispatcher.InvokeAsync(f).Task;

        /// <summary>
        /// Runs the given Action on the Dispatcher.
        /// </summary>
        /// <param name="a">The Action to run.</param>
        /// <returns>A Task to perform the given Action.</returns>
        public Task RunAsync(Action a) =>
            _dispatcher.InvokeAsync(a).Task;
    }
}
