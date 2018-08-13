using System.Linq;
using System.Threading.Tasks;
using EsapiPowerTools.Async;
using VMS.TPS.Common.Model.API;

namespace EsapiPowerTools.SampleServiceImpl
{
    public class EclipsePatientSession : PatientSessionAsyncBase
    {
        public EclipsePatientSession(Patient patient, Application app, EsapiAsyncRunner runner) : base(patient, app, runner)
        { }

        public Task<string[]> GetCoursesAsync() =>
            RunAsync(patient => patient.Courses.Select(x => x.Id).ToArray());
    }
}