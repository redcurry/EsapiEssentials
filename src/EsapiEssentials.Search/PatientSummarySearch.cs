using System;
using System.Collections.Generic;
using System.Linq;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Search
{
    public class PatientSummarySearch
    {
        public IEnumerable<PatientSummary> FindMatches(
            string searchText, IEnumerable<PatientSummary> patientSummaries, int maximumResults)
        {
            if (searchText == null)
                throw new ArgumentNullException(nameof(searchText));

            if (patientSummaries == null)
                throw new ArgumentNullException(nameof(patientSummaries));

            return patientSummaries
                .Where(ps => IsMatch(ps, searchText))
                .Take(maximumResults);
        }

        private bool IsMatch(PatientSummary patient, string searchText)
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

        private bool IsMatchWithOneSearchTerm(PatientSummary patient, string term) =>
            IsSubstring(patient.Id, term) ||
            IsSubstring(patient.LastName, term) ||
            IsSubstring(patient.FirstName, term);

        private bool IsMatchWithTwoSearchTerms(PatientSummary patient, string term1, string term2) =>
            IsMatchWithLastThenFirstName(patient, term1, term2) ||
            IsMatchWithLastThenFirstName(patient, term2, term1);

        private bool IsMatchWithLastThenFirstName(PatientSummary patient, string lastName, string firstName) =>
            IsSubstring(patient.LastName, lastName) && IsSubstring(patient.FirstName, firstName);

        private bool IsSubstring(string small, string large) =>
            large.ToUpper().Contains(small.ToUpper());
    }
}
