using System.Collections.Generic;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials
{
    public class PluginScriptContext
    {
        public PluginScriptContext() { }

        public PluginScriptContext(ScriptContext context)
        {
            CurrentUser = context.CurrentUser;
            Course = context.Course;
            Image = context.Image;
            StructureSet = context.StructureSet;
            Patient = context.Patient;
            PlanSetup = context.PlanSetup;
            ExternalPlanSetup = context.ExternalPlanSetup;
            BrachyPlanSetup = context.BrachyPlanSetup;
            PlansInScope = context.PlansInScope;
            ExternalPlansInScope = context.ExternalPlansInScope;
            BrachyPlansInScope = context.BrachyPlansInScope;
            PlanSumsInScope = context.PlanSumsInScope;
            ApplicationName = context.ApplicationName;
            VersionInfo = context.VersionInfo;
        }

        public User CurrentUser { get; set; }
        public Course Course { get; set; }
        public Image Image { get; set; }
        public StructureSet StructureSet { get; set; }
        public Patient Patient { get; set; }
        public PlanSetup PlanSetup { get; set; }
        public ExternalPlanSetup ExternalPlanSetup { get; set; }
        public BrachyPlanSetup BrachyPlanSetup { get; set; }
        public IEnumerable<PlanSetup> PlansInScope { get; set; }
        public IEnumerable<ExternalPlanSetup> ExternalPlansInScope { get; set; }
        public IEnumerable<BrachyPlanSetup> BrachyPlansInScope { get; set; }
        public IEnumerable<PlanSum> PlanSumsInScope { get; set; }
        public string ApplicationName { get; set; }
        public string VersionInfo { get; set; }

        public static PluginScriptContext From(string[] args, Application app)
        {
            var scriptContextArgs = ScriptContextArgs.From(args);
            return scriptContextArgs?.ToScriptContext(app);
        }
    }
}