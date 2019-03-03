using EsapiEssentials.Plugin;

namespace EsapiEssentials.PluginRunner
{
    /// <summary>
    /// Provides the functionality to start the PluginRunner and run your script.
    /// </summary>
    public static class ScriptRunner
    {
        /// <summary>
        /// Runs the plugin runner for the given script (accepts a Window).
        /// </summary>
        /// <param name="script">The script to run. This script accepts the Window provided by Eclipse.</param>
        /// <param name="userId">The optional user ID to log in to Eclipse.</param>
        /// <param name="password">The optional user password to log in to Eclipse.</param>
        public static void Run(ScriptBase script, string userId = null, string password = null) =>
            RunWith(new PluginRunner(script, userId, password));

        /// <summary>
        /// Runs the plugin runner for the given script (does not accept a Window).
        /// </summary>
        /// <param name="script">The script to run. This script does not use an Eclipse-provided Window.</param>
        /// <param name="userId">The optional user ID to log in to Eclipse.</param>
        /// <param name="password">The optional user password to log in to Eclipse.</param>
        public static void Run(ScriptBaseWithoutWindow script, string userId = null, string password = null) =>
            RunWith(new PluginRunner(script, userId, password));

        private static void RunWith(PluginRunner runner)
        {
            var vm = new MainViewModel(runner);
            var window = new MainWindow(vm);
            window.Closed += (o, e) => runner.Dispose();
            window.Show();
        }
    }
}
