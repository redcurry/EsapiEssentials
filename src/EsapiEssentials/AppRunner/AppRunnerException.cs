using System;

namespace EsapiEssentials
{
    public class AppRunnerException : Exception
    {
        public AppRunnerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}