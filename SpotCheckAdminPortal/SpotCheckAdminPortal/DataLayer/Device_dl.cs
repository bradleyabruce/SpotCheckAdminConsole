using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SpotCheckAdminPortal.Models;

namespace SpotCheckAdminPortal.DataLayer
{
   public class Device_dl : Device
   {
      public Device_dl(Device device)
      {
         this.DeviceID = device.DeviceID;
         this.DeviceName = device.DeviceName;
         this.LocalIpAddress = device.LocalIpAddress;
         this.ExternalIpAddress = device.ExternalIpAddress;
         this.MacAddress = device.MacAddress;
         this.LastUpdateDate = device.LastUpdateDate;
         this.CompanyID = device.CompanyID;
         this.TakeNewImage = device.TakeNewImage;
         this.DeviceStatusID = device.DeviceStatusID;
         this.ParkingLotID = device.ParkingLotID;
      }

      #region GetDeviceListFromCompanyID

      public new List<Device> GetDeviceListFromCompanyID(int companyID)
      {
         List<Device> devices = new List<Device>();

         string url = IoC.API_URL + "device/getDevicesByCompanyID";
         string json = companyID.ToString();

         HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);

         if (request != null)
         {
            Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
            HttpStatusCode code = response.FirstOrDefault().Key;
            string httpResponse = response.FirstOrDefault().Value;

            if (code == HttpStatusCode.OK)
            {
               //return devices
               devices = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Device>>(httpResponse);
               return devices;
            }
            else
            {
               if (httpResponse == "No devices are linked to this company.")
               {
                  //Return empty list
                  return devices;
               }
               else
               {
                  //return null
                  return null;
               }
            }
         }
         else
         {
            //return null
            return null;
         }
      }

      #endregion GetDeviceListFromCompanyID
   }
}
