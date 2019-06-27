using System;
using System.Linq;
using System.Threading.Tasks;
using EsapiEssentials.Plugin;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Samples.AsyncPlugin
{
    public class EsapiService : IEsapiService
    {
        private readonly DispatcherRunner _runner;
        private readonly PluginScriptContext _context;
        private readonly DoseMetricCalculator _metricCalc;

        public EsapiService(DispatcherRunner runner, PluginScriptContext context)
        {
            _runner = runner;
            _context = context;
            _metricCalc = new DoseMetricCalculator();
        }

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

        protected Task<T> RunAsync<T>(Func<Patient, T> f) =>
            RunAsync(() => f(_context.Patient));

        protected Task RunAsync(Action a) => _runner.RunAsync(a);
        protected Task<T> RunAsync<T>(Func<T> f) => _runner.RunAsync(f);

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