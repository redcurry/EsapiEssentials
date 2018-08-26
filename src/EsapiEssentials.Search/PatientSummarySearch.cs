using System;
using System.Collections.Generic;
using System.Linq;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Search
{
    internal class PatientSummarySearchInternal
    {
        public IEnumerable<SearchPatient> FindMatches(
            string searchText, IEnumerable<SearchPatient> patients, int maximumResults) =>
            patients.Where(p => IsMatch(p, searchText)).Take(maximumResults);

        private bool IsMatch(SearchPatient patient, string searchText)
        {
            if (searchText.Length < 1)
                return false;

            var searchTerms = GetSearchTerms(searchText);
            switch (searchTerms.Length)
            {
                case 0:
                    return false;
                case 1:
                    return IsMatchWithOneSearchTerm(patient, searchTerms[0]);
                default:
                    return IsMatchWithTwoSearchTerms(patient, searchTerms[0], searchTerms[1]);
            }
        }

        private string[] GetSearchTerms(string searchText) =>
            searchText.Split().Select(term => term.Trim(',', ';')).ToArray();

        private bool IsMatchWithOneSearchTerm(SearchPatient patient, string term) =>
            IsSubstring(term, patient.Id) ||
            IsSubstring(term, patient.LastName) ||
            IsSubstring(term, patient.FirstName);

        private bool IsMatchWithTwoSearchTerms(SearchPatient patient, string term1, string term2) =>
            IsMatchWithLastThenFirstName(patient, term1, term2) ||
            IsMatchWithLastThenFirstName(patient, term2, term1);

        private bool IsMatchWithLastThenFirstName(SearchPatient patient, string lastName, string firstName) =>
            IsSubstring(lastName, patient.LastName) && IsSubstring(firstName, patient.FirstName);

        private bool IsSubstring(string small, string large) =>
            large.ToUpper().Contains(small.ToUpper());
    }

    public class PatientSummarySearch
    {
        private readonly PatientSummarySearchInternal _search;

        public PatientSummarySearch()
        {
            _search = new PatientSummarySearchInternal();
        }

        public IEnumerable<PatientSummary> FindMatches(
            string searchText, IEnumerable<PatientSummary> patientSummaries, int maximumResults)
        {
            if (searchText == null)
                throw new ArgumentNullException(nameof(searchText));

            if (patientSummaries == null)
                throw new ArgumentNullException(nameof(patientSummaries));

            var matchingIds = _search.FindMatches(searchText, GetSearchPatients(patientSummaries), maximumResults).Select(x => x.Id);
            return GetPatientSummariesWithIds(patientSummaries, matchingIds);
        }

        private IEnumerable<PatientSummary> GetPatientSummariesWithIds(IEnumerable<PatientSummary> patients, IEnumerable<string> ids) =>
            patients.Where(ps => ids.Contains(ps.Id));

        private IEnumerable<SearchPatient> GetSearchPatients(IEnumerable<PatientSummary> patients) =>
            patients.Select(ps => new SearchPatient {Id = ps.Id, FirstName = ps.FirstName, LastName = ps.LastName});
    }
}
