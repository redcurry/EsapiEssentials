using System.Threading.Tasks;

namespace EsapiEssentials.Samples.Async
{
    public interface IEsapiService
    {
        Task LogInAsync();
        Task LogInAsync(string userId, string password);

        Task OpenPatientAsync(string patientId);
        Task ClosePatientAsync();

        Task<string[]> SearchAsync(string searchText);

        Task<string[]> GetCourseIdsAsync();

        Task<double> CalculateMetricAsync(string metric, string courseId, string planId, string structureId);
    }
}
