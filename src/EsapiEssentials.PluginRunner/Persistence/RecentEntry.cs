using System;
using System.Collections.Generic;
using System.Linq;

namespace EsapiEssentials.PluginRunner
{
    internal class RecentEntry : IEquatable<RecentEntry>
    {
        public string PatientId { get; set; }
        public List<PlanOrPlanSum> PlansAndPlanSumsInScope { get; set; }
        public PlanOrPlanSum ActivePlan { get; set; }

        public bool Equals(RecentEntry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(PatientId, other.PatientId) &&
                   Enumerable.SequenceEqual(PlansAndPlanSumsInScope, other.PlansAndPlanSumsInScope) &&
                   Equals(ActivePlan, other.ActivePlan);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RecentEntry)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (PatientId != null ? PatientId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PlansAndPlanSumsInScope != null ? PlansAndPlanSumsInScope.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ActivePlan != null ? ActivePlan.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(RecentEntry r1, RecentEntry r2) =>
            Equals(r1, r2);

        public static bool operator !=(RecentEntry r1, RecentEntry r2) =>
            !(r1 == r2);
    }
}