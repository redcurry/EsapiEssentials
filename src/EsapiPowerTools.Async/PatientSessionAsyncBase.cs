using System;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace EsapiPowerTools.Async
{
    public abstract class PatientSessionAsyncBase
    {
        private readonly Patient _patient;
        private readonly Application _app;
        private readonly EsapiAsyncRunner _runner;

        protected PatientSessionAsyncBase(Patient patient, Application app, EsapiAsyncRunner runner)
        {
            _patient = patient;
            _app = app;
            _runner = runner;
        }

        protected Task RunAsync(Action<Patient> a) =>
            _runner.RunAsync(() => a(_patient));

        protected Task<T> RunAsync<T>(Func<Patient, T> f) =>
            _runner.RunAsync(() => f(_patient));

        protected Task ClosePatientAsync() =>
            _runner.RunAsync(() => _app.ClosePatient());
    }
}