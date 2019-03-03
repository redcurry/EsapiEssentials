using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Standalone
{
    /// <summary>
    /// Allows a binary plugin script to run a standalone application,
    /// while preserving the script context (sent via command-line arguments).
    /// The plugin script assembly must be in the same directory as the
    /// standalone application. One way to do that is to reference
    /// the script project from the standalone application project.
    /// </summary>
    public class StandaloneRunner
    {
        /// <summary>
        /// Run the standalone application with the given script context.
        /// </summary>
        /// <param name="context">The script context from the plugin script.</param>
        /// <exception cref="StandaloneRunnerException">
        /// An exception occurred while trying to run the standalone application.
        /// Perhaps the application was not found or an error occurred while
        /// building the command-line arguments from the script context.
        /// </exception>
        public static void RunWith(ScriptContext context)
        {
            try
            {
                var contextArgs = ScriptContextArgs.From(context);
                Process.Start(GetStandalonePath(), contextArgs.ToArgs());
            }
            catch (Exception)
            {
                throw new StandaloneRunnerException("Unable to launch the application.");
            }
        }

        private static string GetStandalonePath() =>
            FirstExeIn(DirectoryOf(Assembly()));

        private static string FirstExeIn(string dir) =>
            Directory.GetFiles(dir, "*.exe").First();

        private static string DirectoryOf(Assembly asm) =>
            Path.GetDirectoryName(asm.Location);

        private static Assembly Assembly() =>
            System.Reflection.Assembly.GetExecutingAssembly();
    }
}
