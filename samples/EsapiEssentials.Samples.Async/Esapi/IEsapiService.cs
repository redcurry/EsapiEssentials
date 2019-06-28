using System.Threading.Tasks;

namespace EsapiEssentials.Samples.Async
{
    // Interface to ESAPI functionality while not exposing ESAPI objects.
    // This allows the interface to be passed to other projects
    // without requiring those objects to depend on ESAPI.
    // Also, this interface allows for mocking in unit testing.
    // Finally, all the methods are asynchronous because the implementation
    // uses a separate thread to run ESAPI methods.
    // This prevents slow ESAPI methods from blocking the GUI thread.
    public interface IEsapiService
    {
        Task LogInAsync();
#if ESAPI_13
        Task LogInAsync(string userId, string password);
#endif

        Task OpenPatientAsync(string patientId);
        Task ClosePatientAsync();

        Task<PatientMatch[]> SearchAsync(string searchText);

        Task<Plan[]> GetPlansAsync();
        Task<string[]> GetStructureIdsAsync(string courseId, string planId);

        Task<double> CalculateMeanDoseAsync(string courseId, string planId, string structureId);
    }
}
