using System.Linq;
using System.Threading.Tasks;
using EsapiPowerTools.Async;
using EsapiPowerTools.DoseMetrics;
using EsapiPowerTools.SampleServiceInterface;

namespace EsapiPowerTools.SampleServiceImpl
{
    public class EsapiService : EsapiServiceBase, IEsapiService
    {
        private readonly DoseMetricCalculator _metricCalc;

        public EsapiService()
        {
            _metricCalc = new DoseMetricCalculator();
        }

        public Task<string[]> GetCourseIdsAsync() =>
            RunAsync(patient => patient.Courses.Select(x => x.Id).ToArray());

        public Task<double> CalculateMetricAsync(string metric, string courseId, string planId, string structureId) =>
            RunAsync(patient => _metricCalc.Calculate(metric, patient, courseId, planId, structureId));
    }
}
