using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotCheckAdminPortal.Enums
{
   public class eDeviceStatus
   {
      public enum DeviceStatus
      {
         NoCompany = 1,
         Undeployed = 2,
         WaitingForImage = 3,
         ReadyForSpots = 4,
         Deployed = 5,
      }
   }
}