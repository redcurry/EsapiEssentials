namespace EsapiEssentials.PluginRunner
{
    public static class ScriptRunner
    {
        public static void Run(ScriptBase script) =>
            RunWith(new PluginRunner(script));

        public static void Run(ScriptBaseWithoutWindow script) =>
            RunWith(new PluginRunner(script));

        private static void RunWith(PluginRunner runner)
        {
            var vm = new MainViewModel(runner);
            var window = new MainWindow(vm);
            window.Closed += (o, e) => runner.Dispose();
            window.Show();
        }
    }
}
