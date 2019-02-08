using System.Windows;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials
{
    public abstract class ScriptBase
    {
        public void Execute(ScriptContext context, Window window)
        {
            Execute(new PluginScriptContext(context), window);
        }

        public abstract void Execute(PluginScriptContext context, Window window);
    }
}
