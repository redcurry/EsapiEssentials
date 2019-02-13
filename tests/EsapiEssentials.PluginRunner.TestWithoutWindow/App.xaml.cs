using System.Windows;

namespace EsapiEssentials.PluginRunner.TestWithoutWindow
{
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ScriptRunner.Run(new Script());
        }
    }
}
