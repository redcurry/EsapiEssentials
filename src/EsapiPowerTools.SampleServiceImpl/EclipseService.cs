using System.Threading.Tasks;
using EsapiPowerTools.DoseMetrics;
using EsapiPowerTools.SampleServiceInterface;

namespace EsapiPowerTools.SampleServiceImpl
{
    public class EclipseService
    {
        public Task<EclipseAppSession> LogInAsync()
        {

        }
    }

    public class EclipseAppSession
    {
        public Task<EclipsePatientSession> OpenPatient(string patientId)
        {
        }
    }
}
