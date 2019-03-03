using EsapiEssentials.Plugin;

namespace EsapiEssentials.PluginRunner
{
    public static class ScriptRunner
    {
        public static void Run(ScriptBase script, string userId = null, string password = null) =>
            RunWith(new PluginRunner(script, userId, password));

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
