using EsapiEssentials.PluginRunner;
using System.Windows;
using VMS.TPS;

namespace EsapiEssentials.Samples.AsyncPlugin.Runner
{
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            // Note: EsapiEssentials and EsapiEssentials.PluginRunner must be referenced,
            // as well as the project that contains the Script class
            ScriptRunner.Run(new Script());
        }
    }
}
