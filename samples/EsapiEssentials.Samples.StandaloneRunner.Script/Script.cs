using EsapiEssentials;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS
{
    public class Script
    {
        public void Execute(ScriptContext context)
        {
            AppRunner.RunWith(context);
        }
    }
}
