using System;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Standalone
{
    /// <summary>
    /// Provides asynchronous methods to access ESAPI from a separate thread.
    /// This class is abstract, so it must be sub-classed in order to be used.
    /// </summary>
    public abstract class EsapiServiceBase : IDisposable
    {
        private readonly AsyncRunner _runner;

        private Application _app;
        private Patient _patient;

        protected EsapiServiceBase()
        {
            _runner = new AsyncRunner();
        }

        /// <summary>
        /// Creates the ESAPI Application object without user credentials,
        /// so a log-in dialog box will be displayed to the user.
        /// </summary>
        /// <returns>A Task to perform this action.</returns>
        public virtual Task LogInAsync() =>
#if ESAPI_13
            RunAsync(() => _app = Application.CreateApplication(null, null));
#elif ESAPI_15
            RunAsync(() => _app = Application.CreateApplication());
#endif

#if ESAPI_13
        /// <summary>
        /// Creates the ESAPI Application object with the given user credentials,
        /// so a log-in dialog box will not be displayed to the user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="password">The user password.</param>
        /// <returns>A Task to perform this action.</returns>
        public virtual Task LogInAsync(string userId, string password) =>
            RunAsync(() => _app = Application.CreateApplication(userId, password));
#endif

        /// <summary>
        /// Opens the patient for the given patient ID.
        /// </summary>
        /// <param name="patientId">The ID of the patient to open.</param>
        /// <returns>A Task to perform this action.</returns>
        /// <exception cref="LogInRequiredException">
        /// The user has not logged in and therefore the ESAPI Application object
        /// has not been initialized.
        /// </exception>
        public virtual Task OpenPatientAsync(string patientId) =>
            RunAsync(() =>
            {
                ValidateApplication();
                _patient = _app.OpenPatientById(patientId);
            });

        /// <summary>
        /// Closes the opened patient.
        /// If there is no opened patient, nothing happens.
        /// </summary>
        /// <returns>A Task to perform this action</returns>
        /// <exception cref="LogInRequiredException">
        /// The user has not logged in and therefore the ESAPI Application object
        /// has not been initialized.
        /// </exception>
        public virtual Task ClosePatientAsync() =>
            RunAsync(() =>
            {
                ValidateApplication();
                _app.ClosePatient();
                _patient = null;
            });

        /// <summary>
        /// Disposes this object.
        /// This method must be called before exiting the application.
        /// </summary>
        public virtual void Dispose()
        {
            _runner.RunAsync(() => _app?.Dispose());
            _runner.Dispose();
        }

        /// <summary>
        /// Runs the given Action on the ESAPI thread.
        /// The Action takes in an ESAPI Application object.
        /// </summary>
        /// <param name="a">The Action to run.</param>
        /// <returns>A Task to perform this action.</returns>
        /// <exception cref="LogInRequiredException">
        /// The user has not logged in and therefore the ESAPI Application object
        /// has not been initialized.
        /// </exception>
        protected Task RunAsync(Action<Application> a) =>
            RunAsync(() =>
            {
                ValidateApplication();
                a(_app);
            });

        /// <summary>
        /// Runs the given Func on the ESAPI thread.
        /// The Func takes in an ESAPI Application object and returns a custom result.
        /// </summary>
        /// <typeparam name="T">The type of the returned object.</typeparam>
        /// <param name="f">The Func to run.</param>
        /// <returns>A Task to perform this action.</returns>
        protected Task<T> RunAsync<T>(Func<Application, T> f) =>
            RunAsync(() =>
            {
                ValidateApplication();
                return f(_app);
            });

        /// <summary>
        /// Runs the given Action on the ESAPI thread.
        /// The Action takes in an ESAPI Patient object.
        /// </summary>
        /// <param name="a">The Action to run.</param>
        /// <returns>A Task to perform this action.</returns>
        /// <exception cref="PatientNotOpenedException">
        /// No patient has been opened.
        /// </exception>
        protected Task RunAsync(Action<Patient> a) =>
            RunAsync(() =>
            {
                ValidatePatient();
                a(_patient);
            });

        /// <summary>
        /// Runs the given Func on the ESAPI thread.
        /// The Func takes in an ESAPI Patient and returns a custom result.
        /// </summary>
        /// <typeparam name="T">The type of the custom result.</typeparam>
        /// <param name="f">The Func to run.</param>
        /// <returns>A Task to perform this action.</returns>
        /// <exception cref="PatientNotOpenedException">
        /// No patient has been opened.
        /// </exception>
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
