using VMS.TPS.Common.Model.API;

namespace EsapiEssentials
{
    public abstract class ScriptBaseWithoutWindow
    {
        public void Execute(ScriptContext context)
        {
            Execute(new PluginScriptContext(context));
        }

        public abstract void Execute(PluginScriptContext context);
    }
}