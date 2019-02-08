using System.Collections.Generic;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials
{
    public class PluginScriptContext
    {
        private readonly ScriptContext _context;

        public PluginScriptContext(ScriptContext context)
        {
            _context = context;
        }

        public User CurrentUser => _context.CurrentUser;
        public Course Course => _context.Course;
        public Image Image => _context.Image;
        public StructureSet StructureSet => _context.StructureSet;
        public Patient Patient => _context.Patient;
        public PlanSetup PlanSetup => _context.PlanSetup;
        public ExternalPlanSetup ExternalPlanSetup => _context.ExternalPlanSetup;
        public BrachyPlanSetup BrachyPlanSetup => _context.BrachyPlanSetup;
        public IEnumerable<PlanSetup> PlansInScope => _context.PlansInScope;
        public IEnumerable<ExternalPlanSetup> ExternalPlansInScope => _context.ExternalPlansInScope;
        public IEnumerable<BrachyPlanSetup> BrachyPlansInScope => _context.BrachyPlansInScope;
        public IEnumerable<PlanSum> PlanSumsInScope => _context.PlanSumsInScope;
        public string ApplicationName => _context.ApplicationName;
        public string VersionInfo => _context.VersionInfo;
    }
}