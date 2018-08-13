using System;

namespace EsapiPowerTools.Facade
{
    public class PatientNotFoundException : Exception
    {
        public PatientNotFoundException(string patientId)
            : base($"The patient {patientId} was not found.") { }
    }
}