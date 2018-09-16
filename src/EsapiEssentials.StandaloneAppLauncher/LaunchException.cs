using System;

namespace EsapiEssentials.StandaloneAppLauncher
{
    public class LaunchException : Exception
    {
        public LaunchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}