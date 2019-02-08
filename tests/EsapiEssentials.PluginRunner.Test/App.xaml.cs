using System.Windows;
using VMS.TPS;

namespace EsapiEssentials.PluginRunner.Test
{
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ScriptRunner.Run(new Script());
        }
    }
}
