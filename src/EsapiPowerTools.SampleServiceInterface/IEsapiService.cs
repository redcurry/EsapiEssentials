using System.Threading.Tasks;

namespace EsapiPowerTools.SampleServiceInterface
{
    public interface IEsapiService
    {
        Task LogInAsync();
        Task OpenPatientAsync(string patientId);
        Task ClosePatientAsync();

        Task<string[]> GetCourseIdsAsync();
        Task<double> CalculateMetricAsync(string metric, string courseId, string planId, string structureId);
    }
}
