using System;

namespace EsapiEssentials.Standalone
{
    public class LogInRequiredException : Exception
    {
        public LogInRequiredException() { }

        public LogInRequiredException(Exception innerException)
            : base("The user has not logged in to Eclipse.", innerException) { }
    }
}