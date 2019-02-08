using System;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace EsapiEssentials.Samples.EsapiService
{
    public class DoseMetricCalculator
    {
        public double Calculate(string metric, PlanningItem planningItem, Structure structure)
        {
            Validate(planningItem);
            Validate(structure);

            var dvhResult = planningItem.GetDVHCumulativeData(
                structure, DoseValuePresentation.Absolute, VolumePresentation.AbsoluteCm3, 0.001);

            // Return the mean dose for now
            return dvhResult?.MeanDose.Dose ?? double.NaN;
        }

        private void Validate(PlanningItem planningItem)
        {
            if (planningItem == null)
                throw new ArgumentNullException(nameof(planningItem));
        }

        private void Validate(Structure structure)
        {
            if (structure == null)
                throw new ArgumentNullException(nameof(structure));
        }
   }
}
