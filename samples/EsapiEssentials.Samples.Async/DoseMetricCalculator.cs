using System;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace EsapiEssentials.Samples.Async
{
    // This class works directly with ESAPI objects, but it will be wrapped by EsapiService,
    // which doesn't expose ESAPI objects in order to isolate the app from ESAPI
    public class DoseMetricCalculator
    {
        public double CalculateMean(PlanningItem planningItem, Structure structure)
        {
            try
            {
                var dvhResult = planningItem.GetDVHCumulativeData(
                    structure, DoseValuePresentation.Absolute, VolumePresentation.AbsoluteCm3, 0.001);
                return dvhResult.MeanDose.Dose;
            }
            catch (Exception e)
            {
                // There are many reasons the DVH calculation could fail,
                // so wrap any exception in a general exception
                throw new InvalidOperationException("Unable to calculate the mean dose.", e);
            }
        }
   }
}
