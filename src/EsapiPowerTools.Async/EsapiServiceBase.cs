using System;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace EsapiPowerTools.Async
{
    public abstract class EsapiServiceBase : IDisposable
    {
        private readonly EsapiAsyncRunner _runner;

        private Application _app;
        private Patient _patient;

        protected EsapiServiceBase()
        {
            _runner = new EsapiAsyncRunner();
        }

        public Task LogInAsync() =>
            RunAsync(() => _app = Application.CreateApplication(null, null));

        public Task LogInAsync(string userId, string password) =>
            RunAsync(() => _app = Application.CreateApplication(userId, password));

        public Task OpenPatientAsync(string patientId) =>
            RunAsync(() =>
            {
                ValidateApplication();
                _patient = _app.OpenPatientById(patientId);
            });

        public Task ClosePatientAsync() =>
            RunAsync(() =>
            {
                ValidateApplication();
                _app.ClosePatient();
                _patient = null;
            });

        public void Dispose()
        {
            _runner.RunAsync(() => _app?.Dispose());
            _runner.Dispose();
        }

        protected Task RunAsync(Action<Application> a) =>
            RunAsync(() =>
            {
                ValidateApplication();
                a(_app);
            });

        protected Task<T> RunAsync<T>(Func<Application, T> f) =>
            RunAsync(() =>
            {
                ValidateApplication();
                return f(_app);
            });

        protected Task RunAsync(Action<Patient> a) =>
            RunAsync(() =>
            {
                ValidatePatient();
                a(_patient);
            });

        protected Task<T> RunAsync<T>(Func<Patient, T> f) =>
            RunAsync(() =>
            {
                ValidatePatient();
                return f(_patient);
            });

        private Task RunAsync(Action a) => _runner.RunAsync(a);
        private Task<T> RunAsync<T>(Func<T> f) => _runner.RunAsync(f);

        private void ValidateApplication()
        {
            if (_app == null)
                throw new NotLoggedInException();
        }

        private void ValidatePatient()
        {
            if (_patient == null)
                throw new PatientNotOpenedException();
        }
    }
}
