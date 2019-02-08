namespace EsapiEssentials.PluginRunner
{
    public class ScriptRunner
    {
        public static async void Run(ScriptBase script)
        {
            var esapiService = new EsapiService();
            var app = new PluginRunnerApp(esapiService, script);
            await app.LogInToEsapi();

            var vm = new MainViewModel(app);
            var window = new MainWindow(vm);

            window.Closed += (sender, args) => esapiService.Dispose();

            window.Show();
        }

        public static void Run(ScriptBaseWithoutWindow script)
        {
        }
    }
}
