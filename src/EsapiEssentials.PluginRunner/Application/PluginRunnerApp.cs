using System.Collections.Generic;
using System.Threading.Tasks;

namespace EsapiEssentials.PluginRunner
{
    internal class PluginRunnerApp
    {
        private readonly IEsapiService _esapiService;

        public PluginRunnerApp(IEsapiService esapiService, ScriptBase script)
        {
            _esapiService = esapiService;
        }

        public Task LogInToEsapi() =>
            _esapiService.LogInAsync();

        public async Task<IEnumerable<PatientMatch>> FindPatientMatchesAsync(string searchText) =>
            await _esapiService.SearchAsync(searchText);

        public async Task<IEnumerable<PlanOrPlanSum>> GetPlansAndPlanSumsFor(string patientId)
        {
            await _esapiService.OpenPatientAsync(patientId);
            var plansAndPlanSums = await _esapiService.GetPlansAndPlanSumsAsync();
            await _esapiService.ClosePatientAsync();
            return plansAndPlanSums;
        }
    }
}
