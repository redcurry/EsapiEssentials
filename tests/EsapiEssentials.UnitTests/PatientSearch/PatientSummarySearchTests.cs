using System.Linq;
using EsapiEssentials.Standalone;
using NUnit.Framework;

namespace EsapiEssentials.UnitTests
{
    [TestFixture]
    public class PatientSummarySearchTests
    {
        private SearchPatient[] _patients;

        [SetUp]
        public void SetUp()
        {
            _patients = new[]
            {
                new SearchPatient {Id = "001", FirstName = "Yvonne",    LastName = "McCarthy"},
                new SearchPatient {Id = "002", FirstName = "Marlene",   LastName = "Lyons"},
                new SearchPatient {Id = "003", FirstName = "Carl",      LastName = "Butler"},
                new SearchPatient {Id = "004", FirstName = "Lamar",     LastName = "Nguyen"},
                new SearchPatient {Id = "005", FirstName = "Terence",   LastName = "Hampton"},
                new SearchPatient {Id = "006", FirstName = "Mike",      LastName = "Massey"},
                new SearchPatient {Id = "007", FirstName = "Hernietta", LastName = "Reid"},
                new SearchPatient {Id = "008", FirstName = "Carol",     LastName = "Dawson"},
                new SearchPatient {Id = "009", FirstName = "Katie",     LastName = "Wagner"},
                new SearchPatient {Id = "010", FirstName = "Kelley",    LastName = "Houston"},
            };
        }

        [TestCase("", new string[0])]
        [TestCase(" ", new string[0])]
        [TestCase("car", new[] {"003", "008"})]
        [TestCase("mike", new[] {"006"})]
        [TestCase("robert", new string[0])]
        [TestCase("ngu lam", new[] {"004"})]
        [TestCase("ngu  lam", new[] {"004"})]
        [TestCase("dawson,c", new[] {"008"})]
        public void SearchTest(string searchText, string[] resultIds)
        {
            var search = new PatientSearch(_patients, 10);

            var results = search.FindMatches(searchText).ToArray();

            Assert.That(results.Select(x => x.Id).ToArray(), Is.EqualTo(resultIds));
        }
    }
}
