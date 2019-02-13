using System;

namespace EsapiEssentials.PluginRunner
{
    public static class ScriptRunner
    {
        public static void Run(ScriptBase script)
        {
            var runner = new PluginRunner(script);

            var vm = new MainViewModel(runner);
            var window = new MainWindow(vm);
            window.Closed += (o, e) => runner.Dispose();

            window.Show();
        }

        public static void Run(ScriptBaseWithoutWindow script)
        {
        }
    }
}
