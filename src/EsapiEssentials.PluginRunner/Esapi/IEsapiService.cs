using System.Threading.Tasks;

namespace EsapiEssentials.PluginRunner
{
    internal interface IEsapiService
    {
        Task LogInAsync();
        Task LogInAsync(string userId, string password);

        Task OpenPatientAsync(string patientId);
        Task ClosePatientAsync();

        Task<PatientMatch[]> SearchAsync(string searchText);
    }
}
