using System.Linq;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.PluginRunner
{
    internal class EsapiService : EsapiServiceBase, IEsapiService
    {
        private PatientSummarySearch _search;

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

        public Task<PatientMatch[]> SearchAsync(string searchText) =>
            RunAsync(() => _search.FindMatches(searchText).Select(CreatePatientMatch).ToArray());

        private Task InitializeSearchAsync() =>
            RunAsync(app => _search = new PatientSummarySearch(app.PatientSummaries, 10));

        private PatientMatch CreatePatientMatch(PatientSummary ps) =>
            new PatientMatch
            {
                Id = ps.Id,
                LastName = ps.LastName,
                FirstName = ps.FirstName
            };
    }
}
