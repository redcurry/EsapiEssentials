namespace EsapiEssentials.PluginRunner
{
    public class ScriptRunner
    {
        public static void Run(ScriptBase script)
        {
            var app = new PluginRunnerApp(script);
            app.LogInToEsapi();

            var vm = new MainViewModel(app);
            var window = new MainWindow(vm);

            window.Closed += (sender, args) => app.LogOutFromEsapi();

            window.Show();
        }

        public static void Run(ScriptBaseWithoutWindow script)
        {
        }
    }
}
