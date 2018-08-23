﻿using System;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace EsapiPowerTools.DoseMetrics
{
    public class DoseMetricCalculator
    {
        public double Calculate(string metric, PlanningItem planningItem, Structure structure)
        {
            Validate(planningItem);
            Validate(structure);

            // Return the mean dose for now
            var dvhResult = planningItem.GetDVHCumulativeData(
                structure, DoseValuePresentation.Absolute, VolumePresentation.AbsoluteCm3, 0.001);
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
