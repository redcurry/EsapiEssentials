using System;

namespace EsapiEssentials.Standalone
{
    internal class SearchPatient
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? CreationDateTime { get; set; }
    }
}