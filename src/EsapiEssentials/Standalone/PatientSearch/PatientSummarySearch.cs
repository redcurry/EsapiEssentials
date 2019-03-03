using System.Collections.Generic;
using System.Linq;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Standalone
{
    public class PatientSummarySearch
    {
        private readonly IEnumerable<PatientSummary> _patients;
        private readonly PatientSearch _search;

        /// <summary>
        /// Initializes a new instance of the PatientSummarySearch class.
        /// </summary>
        /// <param name="patients">The list of patients to search.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        public PatientSummarySearch(IEnumerable<PatientSummary> patients, int maxResults)
        {
            // Need to convert patients to an array,
            // or the IEnumerable will be iterated multiple times
            _patients = patients.ToArray();

            // Adapt each PatientSummary to a SearchPatient object
            // to make PatientSearch testable without referencing ESAPI
            var searchPatients = CreateSearchPatients();
            _search = new PatientSearch(searchPatients, maxResults);
        }

        /// <summary>
        /// Finds the patients that match the given search string.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        /// <returns>The patients that match the given search string.</returns>
        /// <remarks>
        /// If the search string is a single word, a patient matches it
        /// if the patient's ID, first name, or last name start with it.
        /// If the search string contains two or more words, a patient matches it
        /// if the patient's first name or last name start with any of the first
        /// two words in the search string (the remaining words are ignored).
        /// The words in a search string may be separated by a space, a comma, or
        /// a semicolon.
        /// </remarks>
        public IEnumerable<PatientSummary> FindMatches(string searchText)
        {
            // Need to convert matches to an array,
            // or the IEnumerable will be iterated multiple times
            var matches = _search.FindMatches(searchText);
            return GetPatientSummaries(matches.ToArray());
        }

        private IEnumerable<SearchPatient> CreateSearchPatients() =>
            _patients.Select(CreateSearchPatient);

        private SearchPatient CreateSearchPatient(PatientSummary patientSummary) =>
            new SearchPatient
            {
                Id = patientSummary.Id,
                FirstName = patientSummary.FirstName,
                LastName = patientSummary.LastName,
                CreationDateTime = patientSummary.CreationDateTime
            };

        private IEnumerable<PatientSummary> GetPatientSummaries(IEnumerable<SearchPatient> patients) =>
            _patients.Where(ps => ContainsById(ps, patients));

        private bool ContainsById(PatientSummary patientSummary, IEnumerable<SearchPatient> patients) =>
            patients.FirstOrDefault(p => p.Id == patientSummary.Id) != null;
    }
}
