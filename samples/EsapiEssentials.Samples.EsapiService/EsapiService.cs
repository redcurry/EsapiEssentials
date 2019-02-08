using System.Linq;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Samples.EsapiService
{
    public class EsapiService : EsapiServiceBase, IEsapiService
    {
        private readonly DoseMetricCalculator _metricCalc;

        private PatientSummarySearch _search;

        public EsapiService()
        {
            _metricCalc = new DoseMetricCalculator();
        }

        public override async Task LogInAsync()
        {
            await base.LogInAsync();
            await InitializeSearchAsync();
        }

        public override async Task LogInAsync(string userId, string password)
        {
            await base.LogInAsync(userId, password);
            await InitializeSearchAsync();
        }

        public Task<string[]> SearchAsync(string searchText) =>
            RunAsync(() => _search.FindMatches(searchText).Select(ps => $"{ps.LastName}, {ps.FirstName}").ToArray());

        public Task<string[]> GetCourseIdsAsync() =>
            RunAsync(patient => patient.Courses.Select(x => x.Id).ToArray());

        public Task<double> CalculateMetricAsync(string metric, string courseId, string planId, string structureId) =>
            RunAsync(patient => CalculateMetric(metric, patient, courseId, planId, structureId));

        private Task InitializeSearchAsync() =>
            RunAsync(app => _search = new PatientSummarySearch(app.PatientSummaries, 10));

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
