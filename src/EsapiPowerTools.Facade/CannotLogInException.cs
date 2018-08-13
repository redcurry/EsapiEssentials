using System;

namespace EsapiPowerTools.Facade
{
    public class CannotLogInException : Exception
    {
        public CannotLogInException(Exception innerException)
            : base("Unable to log in to Eclipse.", innerException) { }
    }
}