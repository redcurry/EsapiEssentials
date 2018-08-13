using System.Threading.Tasks;

namespace EsapiPowerTools.SampleServiceInterface
{
    public interface IEclipsePatientSession
    {
        Task ClosePatientAsync();

        Task<string[]> GetCourseIdsAsync();
        Task<string[]> GetPlanIdsAsync(string courseId);

        Task<double> CalculateMetricAsync(string metric, string courseId, string planningItemId, string structureId);
    }
}