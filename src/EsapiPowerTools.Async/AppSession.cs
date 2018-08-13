using System;
using VMS.TPS.Common.Model.API;

namespace EsapiPowerTools.Async
{
    public class AppSession : IDisposable
    {
        public AppSession(Application app)
        {
            App = app ?? throw new ArgumentNullException(nameof(app));
        }

        public Application App { get; }

        public PatientSession OpenPatient(string patientId)
        {
            try
            {
                App.ClosePatient();
                var patient = App.OpenPatientById(patientId);
                if (patient == null)
                    throw new PatientNotFoundException(patientId);
                return new PatientSession(patient, App);
            }
            catch (Exception e)
            {
                throw new CannotOpenPatientException(patientId, e);
            }
        }

        public void Dispose()
        {
            App?.Dispose();
        }
    }
}