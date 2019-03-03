using System.Windows;
using EsapiEssentials.Plugin;

namespace EsapiEssentials.PluginRunner.TestWithoutWindow
{
    public class Script : ScriptBaseWithoutWindow
    {
        public override void Execute(PluginScriptContext context)
        {
            MessageBox.Show(context.Patient?.Id ?? "No patient opened.");
        }
    }
}
