using System.Linq;
using System.Threading.Tasks;
using EsapiEssentials.Async;
using EsapiEssentials.DoseMetrics;
using EsapiEssentials.SampleServiceInterface;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.SampleServiceImpl
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
            RunAsync(patient => CalculateMetric(metric, patient, courseId, planId, structureId));

        private double CalculateMetric(string metric, Patient patient, string courseId, string planId, string structureId)
        {
            var plan = GetPlan(patient, courseId, planId);
            var structure = GetStructure(plan, structureId);
            return _metricCalc.Calculate(metric, plan, structure);
        }

        private PlanSetup GetPlan(Patient patient, string courseId, string planId) =>
            GetCourse(patient, courseId)?.PlanSetups?.FirstOrDefault(x => x.Id == planId);

        private Course GetCourse(Patient patient, string courseId) =>
            patient?.Courses?.FirstOrDefault(x => x.Id == courseId);

        private Structure GetStructure(PlanSetup plan, string structureId) =>
            plan?.StructureSet?.Structures?.FirstOrDefault(x => x.Id == structureId);
    }
}
