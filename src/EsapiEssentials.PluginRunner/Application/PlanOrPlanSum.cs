using System;

namespace EsapiEssentials.PluginRunner
{
    internal class PlanOrPlanSum : IEquatable<PlanOrPlanSum>
    {
        public PlanType Type { get; set; }
        public string Id { get; set; }
        public string CourseId { get; set; }

        public bool Equals(PlanOrPlanSum other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Type == other.Type && string.Equals(Id, other.Id) && string.Equals(CourseId, other.CourseId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlanOrPlanSum)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)Type;
                hashCode = (hashCode * 397) ^ (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CourseId != null ? CourseId.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(PlanOrPlanSum p1, PlanOrPlanSum p2) =>
            Equals(p1, p2);

        public static bool operator !=(PlanOrPlanSum p1, PlanOrPlanSum p2) =>
            !(p1 == p2);
    }
}