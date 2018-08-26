using System;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Extensions
{
    /// <summary>
    /// Extension methods for the PlanningItem class.
    /// </summary>
    public static class PlanningItemExtensions
    {
        /// <summary>
        /// Gets the parent Course of the given PlanningItem.
        /// </summary>
        /// <param name="planningItem">The PlanningItem whose Course will be obtained.</param>
        /// <returns>The parent Course of the given PlanningItem.</returns>
        public static Course GetCourse(this PlanningItem planningItem)
        {
            if (planningItem is PlanSetup plan)
                return plan.Course;

            if (planningItem is PlanSum planSum)
                return planSum.Course;

            throw new ArgumentException(GetUnknownSubclassMessage(planningItem));
        }

        /// <summary>
        /// Gets the StructureSet of the given PlanningItem.
        /// </summary>
        /// <param name="planningItem">The PlanningItem whose StructureSet will be obtained.</param>
        /// <returns>The StructureSet of the given PlanningItem.</returns>
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
