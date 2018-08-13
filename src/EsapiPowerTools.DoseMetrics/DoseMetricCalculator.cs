using System.Threading.Tasks;

namespace EsapiPowerTools.DoseMetrics
{
    public class DoseMetricCalculator
    {
        public Task<double> CalculateAsync(string planningItemId, string structureId, string metric)
        {
            return Task.FromResult(0.0);
        }
    }
}
