using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.StandaloneAppLauncher
{
    public class Launcher
    {
        public static void Launch(ScriptContext context)
        {
            try
            {
                Process.Start(FirstExeIn(AssemblyDir(Assembly.GetExecutingAssembly())));
            }
            catch (Exception e)
            {
                throw new LaunchException("Unable to launch application.", e);
            }
        }

        private static string FirstExeIn(string dir)
        {
            return Directory.GetFiles(dir, "*.exe").First();
        }

        private static string AssemblyDir(Assembly asm)
        {
            return Path.GetDirectoryName(asm.Location);
        }
    }
}
