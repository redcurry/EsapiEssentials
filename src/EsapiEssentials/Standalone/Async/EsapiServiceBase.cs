using System;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Standalone
{
    public abstract class EsapiServiceBase : IDisposable
    {
        private readonly AsyncRunner _runner;

        private Application _app;
        private Patient _patient;

        protected EsapiServiceBase()
        {
            _runner = new AsyncRunner();
        }

        public virtual Task LogInAsync() =>
            RunAsync(() => _app = Application.CreateApplication(null, null));

        public virtual Task LogInAsync(string userId, string password) =>
            RunAsync(() => _app = Application.CreateApplication(userId, password));

        public virtual Task OpenPatientAsync(string patientId) =>
            RunAsync(() =>
            {
                ValidateApplication();
                _patient = _app.OpenPatientById(patientId);
            });

        public virtual Task ClosePatientAsync() =>
            RunAsync(() =>
            {
                ValidateApplication();
                _app.ClosePatient();
                _patient = null;
            });

        public virtual void Dispose()
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

        protected Task RunAsync(Action a) => _runner.RunAsync(a);
        protected Task<T> RunAsync<T>(Func<T> f) => _runner.RunAsync(f);

        private void ValidateApplication()
        {
            if (_app == null)
                throw new LogInRequiredException();
        }

        private void ValidatePatient()
        {
            if (_patient == null)
                throw new PatientNotOpenedException();
        }
    }
}
