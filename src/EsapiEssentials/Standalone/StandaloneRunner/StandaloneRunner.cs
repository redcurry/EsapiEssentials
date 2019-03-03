using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Standalone
{
    public class StandaloneRunner
    {
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
