using System;
using System.Collections.Generic;
using System.Web.UI;

namespace SpotCheckAdminPortal.Models
{
   public static class IoC
   {
      #region Properties

      public static string API_URL = "http://173.91.255.135:8080/SpotCheckServer-2.1.8.RELEASE/";

      static Company _currentCompany;
      public static Company CurrentCompany
      {
         get
         {
            return _currentCompany;
         }
         set
         {
            _currentCompany = value;
         }
      }

      static List<Device> _deviceList;
      public static List<Device> DeviceList
      {
         get
         {
            return _deviceList;
         }
         set
         {
            _deviceList = value;
         }
      }

      static List<ParkingLot> _parkingLotList;
      public static List<ParkingLot> ParkingLotList
      {
         get
         {
            return _parkingLotList;
         }
         set
         {
            _parkingLotList = value;
         }
      }

        static Tuple<string,string> _pageMessage;
        public static Tuple<string, string> PageMessage
        {
            get
            {
                return _pageMessage;
            }
            set
            {
                _pageMessage = value;
            }
        }

      #endregion

      #region Methods

      public static void ClearSessionIoC()
      {
         CurrentCompany = null;
         DeviceList = null;
         ParkingLotList = null;
      }

      public static bool ValidateInfo()
      {
         bool result = true;

         if (CurrentCompany.CompanyID == 0 || DeviceList == null || ParkingLotList == null)
         {
            result = false;
         }

         return result;
      }
      public static Control FindControlRecursive(Control rootControl, string controlID)
      {
         if (rootControl.ID == controlID) return rootControl;

         foreach (Control controlToSearch in rootControl.Controls)
         {
            Control controlToReturn = FindControlRecursive(controlToSearch, controlID);
            if (controlToReturn != null) return controlToReturn;
         }
         return null;
      }

      #endregion
   }
}
