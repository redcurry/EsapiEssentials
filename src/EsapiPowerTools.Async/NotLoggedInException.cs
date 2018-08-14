using System;

namespace EsapiPowerTools.Async
{
    public class NotLoggedInException : Exception
    {
        public NotLoggedInException() { }

        public NotLoggedInException(Exception innerException)
            : base("The user has not logged in to Eclipse.", innerException) { }
    }
}