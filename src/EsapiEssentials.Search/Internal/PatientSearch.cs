using System.Collections.Generic;
using System.Linq;

namespace EsapiEssentials.Search.Internal
{
    internal class PatientSearch
    {
        private readonly IEnumerable<SearchPatient> _patients;
        private readonly int _maxResults;

        public PatientSearch(IEnumerable<SearchPatient> patients, int maxResults)
        {
            _patients = patients;
            _maxResults = maxResults;
        }

        public IEnumerable<SearchPatient> FindMatches(string searchText) =>
            !string.IsNullOrWhiteSpace(searchText)
                ? _patients.Where(p => IsMatch(p, searchText)).Take(_maxResults)
                : Enumerable.Empty<SearchPatient>();

        private bool IsMatch(SearchPatient patient, string searchText)
        {
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
}