using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Datalogic.API;
using ZgLib;

namespace ZgRemoteApp
{
    public class LaserManager : IDisposable
    {
        private bool Laser_Enabled = false;

        public LaserManager()
        {
        }

        public void Dispose_Laser()
        {
            if (Laser_Enabled)
                Laser_Disable();
        }

        // Enable laser
        public void Laser_Enable()
        {
            if (!Laser_Enabled)
            {
#if DATALOGIC
                Datalogic.API.Device.SetTriggerType(Datalogic.API.Device.TriggerId.Scan, Datalogic.API.Device.TriggerInputType.Barcode);
                Datalogic.API.Device.SetTriggerType(Datalogic.API.Device.TriggerId.Pistol, Datalogic.API.Device.TriggerInputType.Barcode);
#endif
                Laser_Enabled = true;
            }
        }

        // Disable laser
        public void Laser_Disable()
        {
            if (Laser_Enabled)
            {
#if DATALOGIC
                Datalogic.API.Device.SetTriggerType(Datalogic.API.Device.TriggerId.Scan, Datalogic.API.Device.TriggerInputType.None);
                Datalogic.API.Device.SetTriggerType(Datalogic.API.Device.TriggerId.Pistol, Datalogic.API.Device.TriggerInputType.None);
#endif
                Laser_Enabled = false;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose_Laser();
        }

        #endregion
    }
}
