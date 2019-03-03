using System.Windows;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Plugin
{
    /// <summary>
    /// Provides the Execute method that the PluginRunner needs to call.
    /// Any script that will use the PluginRunner should derive from this class.
    /// Derive from this class if you want to use the Window that Eclipse provides.
    /// </summary>
    public abstract class ScriptBase
    {
        /// <summary>
        /// The method that Eclipse calls when the plugin script is started from there.
        /// This method is called automatically, you should never need to deal with it.
        /// </summary>
        public void Execute(ScriptContext context, Window window)
        {
            Execute(new PluginScriptContext(context), window);
        }

        /// <summary>
        /// The method you need to implement for your script to do anything.
        /// </summary>
        /// <param name="context">A copy of the script context of the Eclipse session.</param>
        /// <param name="window">The Window for your script that Eclipse will display.</param>
        public abstract void Execute(PluginScriptContext context, Window window);
    }
}
