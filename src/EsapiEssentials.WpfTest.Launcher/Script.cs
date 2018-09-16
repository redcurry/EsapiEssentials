using System.Reflection;
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
                Launcher.Launch(Assembly.GetExecutingAssembly(), context);
            }
            catch (LaunchException e)
            {
                MessageBox.Show(e.Message, "Launch Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
