using System;
using System.Configuration;
using System.Reflection;

namespace EsapiEssentials.Settings
{
    /// <summary>
    /// Provides read-only access to an assembly's settings.
    /// The settings must be in the assembly's App.config file,
    /// in a section called appSettings (see the Microsoft documentation).
    /// </summary>
    internal class AssemblySettings
    {
        private readonly Assembly _assembly;

        /// <summary>
        /// Instantiates a new instance of the AssemblySettings class.
        /// </summary>
        /// <param name="assembly">The assembly that has the settings.</param>
        public AssemblySettings(Assembly assembly)
        {
            _assembly = assembly;
        }

        /// <summary>
        /// Gets the value of the setting with the given key (i.e., name).
        /// </summary>
        /// <param name="key">The name of the setting.</param>
        /// <returns>The value of the setting.</returns>
        /// <exception cref="InvalidOperationException">
        /// An exception occured while trying to obtain the setting.
        /// </exception>
        public string GetSetting(string key)
        {
            try
            {
                var asmPath = _assembly.Location;
                var config = ConfigurationManager.OpenExeConfiguration(asmPath);
                var setting = config.AppSettings.Settings[key];
                return setting.Value;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unable to read the configuration setting.", e);
            }
        }
    }
}
