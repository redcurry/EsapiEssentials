using System;
using VMS.TPS.Common.Model.API;

namespace EsapiPowerTools.Facade
{
    public class EclipseSystem
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
