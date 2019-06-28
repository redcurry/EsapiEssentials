using System.Linq;
using System.Threading.Tasks;
using EsapiEssentials.Standalone;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Samples.Async
{
    // Sample implementation of IEsapiService using EsapiServiceBase
    public class EsapiService : EsapiServiceBase, IEsapiService
    {
        private readonly DoseMetricCalculator _metricCalc;

        private PatientSummarySearch _search;

        public EsapiService()
        {
            _metricCalc = new DoseMetricCalculator();
        }

        // Override the default LogInAsync functionality
        // so that the patients are obtained after logging in
        public override async Task LogInAsync()
        {
            await base.LogInAsync();
            await InitializeSearchAsync();
        }

#if ESAPI_13
        public override async Task LogInAsync(string userId, string password)
        {
            await base.LogInAsync(userId, password);
            await InitializeSearchAsync();
        }
#endif

        // Use the RunAsync set of methods to run ESAPI-related actions on a separate thread
        public Task<PatientMatch[]> SearchAsync(string searchText) =>
            RunAsync(() => _search.FindMatches(searchText)
                .Select(ps => new PatientMatch
                {
                    Id = ps.Id,
                    FirstName = ps.FirstName,
                    LastName = ps.LastName
                })
                .ToArray());

        public Task<Plan[]> GetPlansAsync() =>
            RunAsync(patient => patient.Courses?
                .SelectMany(x => x.PlanSetups)
                .Select(x => new Plan
                {
                    PlanId = x.Id,
                    CourseId = x.Course?.Id
                })
                .ToArray());

        public Task<string[]> GetStructureIdsAsync(string courseId, string planId) =>
            RunAsync(patient =>
            {
                var plan = GetPlan(patient, courseId, planId);
                return plan?.StructureSet?.Structures?.Select(x => x.Id).ToArray() ?? new string[0];
            });

        public Task<double> CalculateMeanDoseAsync(string courseId, string planId, string structureId) =>
            RunAsync(patient => CalculateMeanDose(patient, courseId, planId, structureId));

        private Task InitializeSearchAsync() =>
            RunAsync(app => _search = new PatientSummarySearch(app.PatientSummaries, 10));

        private double CalculateMeanDose(Patient patient, string courseId, string planId, string structureId)
        {
            var plan = GetPlan(patient, courseId, planId);
            var structure = GetStructure(plan, structureId);
            return _metricCalc.CalculateMean(plan, structure);
        }

        private PlanSetup GetPlan(Patient patient, string courseId, string planId) =>
            GetCourse(patient, courseId)?.PlanSetups?.FirstOrDefault(x => x.Id == planId);

        private Course GetCourse(Patient patient, string courseId) =>
            patient?.Courses?.FirstOrDefault(x => x.Id == courseId);

        private Structure GetStructure(PlanSetup plan, string structureId) =>
            plan?.StructureSet?.Structures?.FirstOrDefault(x => x.Id == structureId);
    }
}
