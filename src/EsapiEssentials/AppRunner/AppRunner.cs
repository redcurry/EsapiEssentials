using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials
{
    public class AppRunner
    {
        public static void RunWith(ScriptContext context)
        {
            try
            {
                var contextArgs = new ScriptContextArgs(context);
                Process.Start(FirstExeIn(DirectoryOf(Assembly())), contextArgs.Args());
            }
            catch (Exception e)
            {
                throw new AppRunnerException("Unable to launch application.", e);
            }
        }

        private static string FirstExeIn(string dir) =>
            Directory.GetFiles(dir, "*.exe").First();

        private static string DirectoryOf(Assembly asm) =>
            Path.GetDirectoryName(asm.Location);

        private static Assembly Assembly() =>
            System.Reflection.Assembly.GetExecutingAssembly();
    }
}
