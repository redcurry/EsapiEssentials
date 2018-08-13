using System;

namespace EsapiPowerTools.Async
{
    public class CannotOpenPatientException : Exception
    {
        public CannotOpenPatientException(string patientId, Exception innerException)
            : base($"Unable to open patient {patientId}.", innerException) { }
    }
}