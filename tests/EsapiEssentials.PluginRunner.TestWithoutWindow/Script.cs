using System.Windows;

namespace EsapiEssentials.PluginRunner.TestWithoutWindow
{
    public class Script : ScriptBaseWithoutWindow
    {
        public override void Execute(PluginScriptContext context)
        {
            MessageBox.Show(context.Patient.Id);
        }
    }
}
