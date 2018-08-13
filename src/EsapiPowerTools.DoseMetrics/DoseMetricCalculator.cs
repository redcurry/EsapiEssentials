using System.Linq;
using System.Threading.Tasks;
using EsapiPowerTools.Async;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace EsapiPowerTools.DoseMetrics
{
    public class DoseMetricCalculator
    {
        private readonly Patient _patient;
        private readonly EsapiAsyncRunner _asyncRunner;

        public DoseMetricCalculator(Patient patient, EsapiAsyncRunner asyncRunner)
        {
            _patient = patient;
            _asyncRunner = asyncRunner;
        }

        public Task<double> CalculateAsync(string courseId, string planningItemId, string structureId, string metric)
        {
            // Return the mean dose for now; TODO: Check for nulls
            return _asyncRunner.RunAsync(() =>
            {
                var course = _patient.Courses?.FirstOrDefault(x => x.Id == courseId);
                var planningItem = course.PlanSetups?.FirstOrDefault(x => x.Id == planningItemId) as PlanningItem ??
                                   course.PlanSums?.FirstOrDefault(x => x.Id == planningItemId);
                Structure structure = null;
                if (planningItem is PlanSetup plan)
                    structure = plan.StructureSet.Structures.FirstOrDefault(x => x.Id == structureId);
                else if (planningItem is PlanSum planSum)
                    structure = planSum.StructureSet.Structures.FirstOrDefault(x => x.Id == structureId);
                var dvhResult = planningItem.GetDVHCumulativeData(structure, DoseValuePresentation.Absolute, VolumePresentation.AbsoluteCm3, 0.001);
                return dvhResult.MeanDose.Dose;
            });
        }
    }
}
