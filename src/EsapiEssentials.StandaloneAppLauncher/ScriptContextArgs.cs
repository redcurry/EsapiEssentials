using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.StandaloneAppLauncher
{
    internal class ScriptContextArgs
    {
        private readonly ScriptContext _context;

        public ScriptContextArgs(ScriptContext context)
        {
            _context = context;
        }

        public string Args()
        {
            return _context.Patient.Id;
        }
    }
}