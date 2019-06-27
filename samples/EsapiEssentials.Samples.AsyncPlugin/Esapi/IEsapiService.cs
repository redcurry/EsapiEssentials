using System.Threading.Tasks;

namespace EsapiEssentials.Samples.AsyncPlugin
{
    public interface IEsapiService
    {
        Task<Plan[]> GetPlansAsync();
        Task<string[]> GetStructureIdsAsync(string courseId, string planId);

        Task<double> CalculateMeanDoseAsync(string courseId, string planId, string structureId);
    }
}