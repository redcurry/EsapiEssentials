using System.Collections.Generic;
using System.Linq;
using EsapiEssentials.Search.Internal;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Search
{
    public class PatientSummarySearch
    {
        private readonly PatientSummary[] _patients;
        private readonly PatientSearch _search;

        public PatientSummarySearch(IEnumerable<PatientSummary> patients, int maxResults)
        {
            _patients = patients.ToArray();
            _search = new PatientSearch(CreateSearchPatients(), maxResults);
        }

        public IEnumerable<PatientSummary> FindMatches(string searchText)
        {
            var matches = _search.FindMatches(searchText);
            return GetPatientSummaries(matches);
        }

        private IEnumerable<SearchPatient> CreateSearchPatients() =>
            _patients.Select(CreateSearchPatient);

        private SearchPatient CreateSearchPatient(PatientSummary patientSummary) =>
            new SearchPatient
            {
                Id = patientSummary.Id,
                FirstName = patientSummary.FirstName,
                LastName = patientSummary.LastName
            };

        private IEnumerable<PatientSummary> GetPatientSummaries(IEnumerable<SearchPatient> patients) =>
            _patients.Where(ps => ContainsById(ps, patients));

        private bool ContainsById(PatientSummary patientSummary, IEnumerable<SearchPatient> patients) =>
            patients.FirstOrDefault(p => p.Id == patientSummary.Id) != null;
    }
}
