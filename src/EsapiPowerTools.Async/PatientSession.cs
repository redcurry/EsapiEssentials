using VMS.TPS.Common.Model.API;

namespace EsapiPowerTools.Async
{
    public class PatientSession
    {
        private readonly Application _app;

        public PatientSession(Patient patient, Application app)
        {
            Patient = patient;
            _app = app;
        }

        public Patient Patient { get; }

        public void ClosePatient()
        {
            _app.ClosePatient();
        }
    }
}