using System.Collections.Generic;

namespace EsapiEssentials.PluginRunner
{
    internal class RecentEntry
    {
        public string PatientId { get; set; }
        public List<PlanOrPlanSum> PlansAndPlanSumsInScope { get; set; }
        public PlanOrPlanSum ActivePlan { get; set; }
    }
}