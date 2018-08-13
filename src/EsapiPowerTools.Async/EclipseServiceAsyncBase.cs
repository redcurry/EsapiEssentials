using System;
using System.Threading.Tasks;
using EsapiPowerTools.Facade;
using VMS.TPS.Common.Model.API;

namespace EsapiPowerTools.Async
{
    public abstract class EclipseServiceAsyncBase
    {
        private readonly EsapiAsyncRunner _runner;
        private readonly EclipseSystem _eclipseSystem;

        protected AppSession _appSession;

        protected EclipseServiceAsyncBase()
        {
            _runner = new EsapiAsyncRunner();
            _eclipseSystem = new EclipseSystem();
        }

        public Task<AppSessionAsyncBase> LogInAsync() =>
            _runner.RunAsync(() =>
            {
                _appSession = _eclipseSystem.LogIn();
                return new AppSessionAsyncBase(_runner);
            });

        protected AppSessionAsyncBase Create
    }

    public abstract class AppSessionAsyncBase
    {
        private readonly AppSession _appSession;
        private readonly EsapiAsyncRunner _runner;

        protected AppSessionAsyncBase(AppSession appSession, EsapiAsyncRunner runner)
        {
            _appSession = appSession;
            _runner = runner;
        }

        protected Task RunAsync(Action<AppSession> a) =>
            _runner.RunAsync(() => a(_appSession));

        protected Task<T> RunAsync<T>(Func<AppSession, T> f) =>
            _runner.RunAsync(() => f(_appSession));

        protected Task<PatientSessionAsyncBase> OpenPatient(string patientId) =>
            _runner.RunAsync(() => CreatePatientSession(_appSession.OpenPatient(patientId)));

        protected abstract PatientSessionAsyncBase CreatePatientSession(PatientSession patientSession);
    }
}
