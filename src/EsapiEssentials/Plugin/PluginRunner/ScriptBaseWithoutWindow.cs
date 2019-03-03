using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Plugin
{
    /// <summary>
    /// Provides the Execute method that the PluginRunner needs to call.
    /// Any script that will use the PluginRunner should derive from this class.
    /// Derive from this class if you're going to create your own Window.
    /// </summary>
    public abstract class ScriptBaseWithoutWindow
    {
        /// <summary>
        /// The method that Eclipse calls when the plugin script is started from there.
        /// This method is called automatically, you should never need to deal with it.
        /// </summary>
        public void Execute(ScriptContext context)
        {
            Execute(new PluginScriptContext(context));
        }

        /// <summary>
        /// The method you need to implement for your script to do anything.
        /// </summary>
        /// <param name="context">A copy of the script context of the Eclipse session.</param>
        public abstract void Execute(PluginScriptContext context);
    }
}