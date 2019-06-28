using System;
using System.Threading.Tasks;

namespace EsapiEssentials.Plugin
{
    public abstract class EsapiServiceBase<TContext>
    {
        private readonly TContext _context;
        private readonly AsyncRunner _runner;

        protected EsapiServiceBase(TContext context)
        {
            _context = context;
            _runner = new AsyncRunner();
        }

        /// <summary>
        /// Runs the given Action on the ESAPI thread.
        /// </summary>
        /// <param name="a">The Action to run.</param>
        /// <returns>A Task to perform this action.</returns>
        protected Task RunAsync(Action<TContext> a) =>
            RunAsync(() => a(_context));

        /// <summary>
        /// Runs the given Func on the ESAPI thread.
        /// </summary>
        /// <typeparam name="T">The type of the returned object.</typeparam>
        /// <param name="f">The Func to run.</param>
        /// <returns>A Task to perform this action.</returns>
        protected Task<T> RunAsync<T>(Func<TContext, T> f) =>
            RunAsync(() => f(_context));

        protected Task RunAsync(Action a) =>
            _runner.RunAsync(a);

        protected Task<T> RunAsync<T>(Func<T> f) =>
            _runner.RunAsync(f);
    }
}