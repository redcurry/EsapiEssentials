using System;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Extensions
{
    public static class PlanningItemExtensions
    {
        public static Course GetCourse(this PlanningItem planningItem)
        {
            if (planningItem is PlanSetup plan)
                return plan.Course;

            if (planningItem is PlanSum planSum)
                return planSum.Course;

            throw new ArgumentException(GetUnknownSubclassMessage(planningItem));
        }

        public static StructureSet GetStructureSet(this PlanningItem planningItem)
        {
            if (planningItem is PlanSetup plan)
                return plan.StructureSet;

            if (planningItem is PlanSum planSum)
                return planSum.StructureSet;

            throw new ArgumentException(GetUnknownSubclassMessage(planningItem));
        }

        private static string GetUnknownSubclassMessage(PlanningItem planningItem) =>
            $"{planningItem.GetType()} is not a known subclass of {nameof(PlanningItem)}.";
    }
}
