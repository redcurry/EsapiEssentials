using System.Collections.Generic;
using System.Linq;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials
{
    /// <summary>
    /// Extension methods for the Patient class.
    /// </summary>
    public static class PatientExtensions
    {
        /// <summary>
        /// Gets every PlanningItem (PlanSetup and PlanSum) of the given Patient.
        /// </summary>
        /// <param name="patient">The Patient.</param>
        /// <returns>Every PlanningItem of the given Patient.</returns>
        public static IEnumerable<PlanningItem> GetPlanningItems(this Patient patient)
        {
            var plans = patient.Courses?.SelectMany(c => c.PlanSetups) ?? new List<PlanSetup>();
            var planSums = patient.Courses?.SelectMany(c => c.PlanSums) ?? new List<PlanSum>();
            return plans.Concat<PlanningItem>(planSums);
        }
    }
}