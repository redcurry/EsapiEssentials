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
    }
}
