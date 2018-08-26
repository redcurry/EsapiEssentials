using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace EsapiEssentials.Search.Tests
{
    [TestFixture]
    public class PatientSummarySearchTests
    {
        private List<SearchPatient> _patients;

        [SetUp]
        public void SetUp()
        {
            _patients = new List<SearchPatient>
            {
                new SearchPatient {Id = "001", FirstName = "Yvonne", LastName = "McCarthy"},
                new SearchPatient {Id = "002", FirstName = "Marlene", LastName = "Lyons"},
                new SearchPatient {Id = "003", FirstName = "Carl", LastName = "Butler"},
                new SearchPatient {Id = "004", FirstName = "Lamar", LastName = "Nguyen"},
                new SearchPatient {Id = "005", FirstName = "Terence", LastName = "Hampton"},
                new SearchPatient {Id = "006", FirstName = "Mike", LastName = "Massey"},
                new SearchPatient {Id = "007", FirstName = "Hernietta", LastName = "Reid"},
                new SearchPatient {Id = "008", FirstName = "Carol", LastName = "Dawson"},
                new SearchPatient {Id = "009", FirstName = "Katie", LastName = "Wagner"},
                new SearchPatient {Id = "010", FirstName = "Kelley", LastName = "Houston"},
            };
        }

        [TestCase("car", new[] {"001", "003", "008"})]
        [TestCase("mike", new[] {"006"})]
        [TestCase("robert", new string[0])]
        [TestCase("", new string[0])]
        public void SearchTest(string searchText, string[] resultIds)
        {
            var search = new PatientSummarySearchInternal();
            var results = search.FindMatches(searchText, _patients, 10).ToArray();

            Assert.That(results.Select(x => x.Id).ToArray(), Is.EqualTo(resultIds));
        }
    }
}
