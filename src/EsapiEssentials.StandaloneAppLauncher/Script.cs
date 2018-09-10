using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS
{
    public class Script
    {
        public void Execute(ScriptContext context)
        {
            try
            {
                var appPath = FindAppPath();
                var appArgs = CreateAppArguments(context);
                Process.Start(appPath, appArgs);
            }
            catch (Exception e)
            {
                MessageBox.Show(CreateErrorMessage(e), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string FindAppPath()
        {
            return FirstExePathIn(GetAssemblyDirectory());
        }

        private string FirstExePathIn(string dir)
        {
            return Directory.GetFiles(dir, "*.exe").First();
        }

        private string GetAssemblyDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        private string CreateAppArguments(ScriptContext context)
        {
            return null;
        }

        private string CreateErrorMessage(Exception e)
        {
            return $"Failed to start the application: {e.Message}";
        }
    }
}
