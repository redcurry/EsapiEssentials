using System.Threading.Tasks;

namespace EsapiPowerTools.SampleServiceInterface
{
    public interface IEclipseSystem
    {
        Task<IEclipseAppSession> LogInAsync();
    }
}
