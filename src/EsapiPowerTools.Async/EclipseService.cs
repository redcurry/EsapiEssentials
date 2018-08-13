using System;
using VMS.TPS.Common.Model.API;

namespace EsapiPowerTools.Async
{
    public class EclipseService
    {
        public AppSession LogIn()
        {
            try
            {
                return new AppSession(Application.CreateApplication(null, null));
            }
            catch (Exception e)
            {
                throw new CannotLogInException(e);
            }
        }
    }
}
