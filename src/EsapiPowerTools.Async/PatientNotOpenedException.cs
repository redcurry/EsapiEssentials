using System;

namespace EsapiPowerTools.Async
{
    public class PatientNotOpenedException : Exception
    {
        public PatientNotOpenedException() { }

        public PatientNotOpenedException(Exception innerException)
            : base("The patient has not been opened.", innerException) { }
    }
}