using System;

namespace EsapiEssentials
{
    public class LogInRequiredException : Exception
    {
        public LogInRequiredException() { }

        public LogInRequiredException(Exception innerException)
            : base("The user has not logged in to Eclipse.", innerException) { }
    }
}