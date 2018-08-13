using System.Threading.Tasks;

namespace EsapiPowerTools.SampleServiceInterface
{
    public interface IEclipseAppSession
    {
        Task<IEclipsePatientSession> OpenPatientAsync(string patientId);
    }
}