using System.Linq;
using System.Threading.Tasks;
using EsapiEssentials.Plugin;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Samples.AsyncPlugin
{
    public class EsapiService : EsapiServiceBase<PluginScriptContext>, IEsapiService
    {
        private readonly DoseMetricCalculator _metricCalc;

        public EsapiService(PluginScriptContext context) : base(context)
        {
            _metricCalc = new DoseMetricCalculator();
        }

        public Task<Plan[]> GetPlansAsync() =>
            RunAsync(context => context.Patient.Courses?
                .SelectMany(x => x.PlanSetups)
                .Select(x => new Plan
                {
                    PlanId = x.Id,
                    CourseId = x.Course?.Id
                })
                .ToArray());

        public Task<string[]> GetStructureIdsAsync(string courseId, string planId) =>
            RunAsync(context =>
            {
                var plan = GetPlan(context.Patient, courseId, planId);
                return plan?.StructureSet?.Structures?.Select(x => x.Id).ToArray() ?? new string[0];
            });

        public Task<double> CalculateMeanDoseAsync(string courseId, string planId, string structureId) =>
            RunAsync(context => CalculateMeanDose(context.Patient, courseId, planId, structureId));

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