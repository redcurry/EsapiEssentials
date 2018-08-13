using System;

namespace EsapiPowerTools.Async
{
    public class CannotLogInException : Exception
    {
        public CannotLogInException(Exception innerException)
            : base("Unable to log in to Eclipse.", innerException) { }
    }
}