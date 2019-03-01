using System.Collections.Generic;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials
{
    internal class ScriptContextArgs
    {
        private readonly ScriptContext _context;

        public ScriptContextArgs(ScriptContext context)
        {
            _context = context;
        }

        public string CurrentUserId { get; set; }
        public string CourseId { get; set; }
        public string ImageId { get; set; }
        public string StructureSetId { get; set; }
        public string PatientId { get; set; }
        public string PlanSetupId { get; set; }
        public string ExternalPlanSetupId { get; set; }
        public string BrachyPlanSetupId { get; set; }
        public IEnumerable<string> PlansInScopeIds { get; set; }
        public IEnumerable<string> ExternalPlansInScopeIds { get; set; }
        public IEnumerable<string> BrachyPlansInScopeIds { get; set; }
        public IEnumerable<string> PlanSumsInScopeIds { get; set; }
        public string ApplicationName { get; set; }
        public string VersionInfo { get; set; }

        public string Args()
        {
            return _context.Patient.Id;
        }
    }
}