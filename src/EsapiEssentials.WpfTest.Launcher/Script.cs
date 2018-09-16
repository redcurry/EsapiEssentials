using System.Windows;
using EsapiEssentials.StandaloneAppLauncher;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS
{
    public class Script
    {
        public void Execute(ScriptContext context)
        {
            try
            {
                Launcher.Launch(context);
            }
            catch (LaunchException e)
            {
                MessageBox.Show(e.Message, "Launch Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
