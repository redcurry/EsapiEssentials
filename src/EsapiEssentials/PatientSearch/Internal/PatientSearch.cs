using System;
using System.Collections.Generic;
using System.Linq;

namespace EsapiEssentials.Internal
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

        public IEnumerable<SearchPatient> FindMatches(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return Enumerable.Empty<SearchPatient>();

            var searchTerms = GetSearchTerms(searchText);
            return _patients
                .Where(p => IsMatch(p, searchTerms))
                .OrderByDescending(p => p.CreationDateTime)
                .Take(_maxResults);
        }

        private bool IsMatch(SearchPatient patient, string[] searchTerms)
        {
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
            large.StartsWith(small, StringComparison.OrdinalIgnoreCase);
    }
}