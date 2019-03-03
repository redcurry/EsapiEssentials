using EsapiEssentials;
using EsapiEssentials.Standalone;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS
{
    public class Script
    {
        public void Execute(ScriptContext context)
        {
            StandaloneRunner.RunWith(context);
        }
    }
}
